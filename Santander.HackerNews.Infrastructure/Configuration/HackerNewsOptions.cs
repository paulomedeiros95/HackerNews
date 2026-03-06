namespace Santander.HackerNews.Infrastructure.Configuration;

/// <summary>
/// Strongly typed configuration for Hacker News integration.
/// </summary>
public class HackerNewsOptions
{
    public const string SectionName = "HackerNewsOptions";

    public string BaseUrl { get; set; } = string.Empty;
    public int MaxStoriesToFetch { get; set; } = 200;
}