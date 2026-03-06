using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Santander.HackerNews.Application.Services;
using Santander.HackerNews.Domain.Interfaces;
using Santander.HackerNews.Infrastructure.BackgroundServices;
using Santander.HackerNews.Infrastructure.Clients;
using Santander.HackerNews.Infrastructure.Configuration;

var builder = WebApplication.CreateBuilder(args);

// 1. Configure strongly typed settings
builder.Services.Configure<HackerNewsOptions>(
    builder.Configuration.GetSection(HackerNewsOptions.SectionName));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();

// 2. Setup Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("Self", () => Microsoft.Extensions.Diagnostics.HealthChecks.HealthCheckResult.Healthy());

builder.Services.AddScoped<IStoryService, StoryService>();

// 3. Configure HttpClient with Polly Resilience and Dynamic BaseUrl
builder.Services.AddHttpClient<IHackerNewsClient, HackerNewsClient>((sp, client) =>
{
    var options = sp.GetRequiredService<IOptions<HackerNewsOptions>>().Value;
    client.BaseAddress = new Uri(options.BaseUrl);
})
.AddStandardResilienceHandler(options =>
{
    // The standard handler automatically handles HTTP 5xx and HTTP 429 (Too Many Requests).
    // We can fine-tune timeouts here if needed.
    options.AttemptTimeout.Timeout = TimeSpan.FromSeconds(5);
    options.CircuitBreaker.SamplingDuration = TimeSpan.FromSeconds(30);
});

builder.Services.AddHostedService<HackerNewsCacheWorker>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

// 4. Map Health Check endpoint
app.MapHealthChecks("/health");

app.Run();