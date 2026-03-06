using System.Text.Json.Serialization;

namespace Santander.HackerNews.Domain.Entities;

/// <summary>
/// Immutable record representing the Hacker News API response.
/// </summary>
public record HackerNewsStory(
    [property: JsonPropertyName("id")] int Id,
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("url")] string Url,
    [property: JsonPropertyName("by")] string By,
    [property: JsonPropertyName("time")] long Time,
    [property: JsonPropertyName("score")] int Score,
    [property: JsonPropertyName("descendants")] int Descendants
);