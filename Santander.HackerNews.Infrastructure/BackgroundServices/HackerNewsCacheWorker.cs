using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Santander.HackerNews.Domain.Dtos;
using Santander.HackerNews.Domain.Entities;
using Santander.HackerNews.Domain.Interfaces;
using Santander.HackerNews.Infrastructure.Configuration;
using System.Collections.Concurrent;

namespace Santander.HackerNews.Infrastructure.BackgroundServices;

public class HackerNewsCacheWorker : BackgroundService
{
    #region Fields
    private readonly IServiceProvider _serviceProvider;
    private readonly IMemoryCache _cache;
    private readonly ILogger<HackerNewsCacheWorker> _logger;
    private readonly HackerNewsOptions _options; 
    private const string CacheKey = "BestStoriesCache";
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(2);
    #endregion

    #region Constructor
    public HackerNewsCacheWorker(
        IServiceProvider serviceProvider,
        IMemoryCache cache,
        ILogger<HackerNewsCacheWorker> logger,
        IOptions<HackerNewsOptions> options)
    {
        _serviceProvider = serviceProvider;
        _cache = cache;
        _logger = logger;
        _options = options.Value;
    }
    #endregion

    #region Methods
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                await RefreshCacheAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error refreshing Hacker News cache.");
            }

            await Task.Delay(_refreshInterval, stoppingToken);
        }
    }

    private async Task RefreshCacheAsync(CancellationToken stoppingToken)
    {
        using var scope = _serviceProvider.CreateScope();
        var client = scope.ServiceProvider.GetRequiredService<IHackerNewsClient>();

        var storyIds = await client.GetBestStoryIdsAsync(stoppingToken);

        // LIMITING THE IDS: We only process the top N configured to save network/CPU and avoid rate limits.
        var bestStoryIds = storyIds.Take(_options.MaxStoriesToFetch).ToList();

        if (!bestStoryIds.Any()) return;

        var stories = new ConcurrentBag<StoryDto>();

        var parallelOptions = new ParallelOptions
        {
            MaxDegreeOfParallelism = 10,
            CancellationToken = stoppingToken
        };

        await Parallel.ForEachAsync(bestStoryIds, parallelOptions, async (id, ct) =>
        {
            var story = await client.GetStoryByIdAsync(id, ct);
            if (story != null && !string.IsNullOrWhiteSpace(story.Title))
            {
                stories.Add(MapToDto(story));
            }
        });

        var orderedStories = stories.OrderByDescending(s => s.Score).ToList();

        _cache.Set(CacheKey, orderedStories);
        _logger.LogInformation("Cache successfully updated with {Count} stories.", orderedStories.Count);
    }

    private static StoryDto MapToDto(HackerNewsStory story)
    {
        return new StoryDto(
            Title: story.Title,
            Uri: story.Url ?? string.Empty,
            PostedBy: story.By,
            Time: DateTimeOffset.FromUnixTimeSeconds(story.Time).ToString("yyyy-MM-ddTHH:mm:sszzz"),
            Score: story.Score,
            CommentCount: story.Descendants
        );
    }
    #endregion
}
