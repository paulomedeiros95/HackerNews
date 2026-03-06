using System.Text.Json.Serialization;

namespace Santander.HackerNews.Domain.Dtos;

/// <summary>
/// Immutable DTO for the API output.
/// </summary>
public record StoryDto(
    [property: JsonPropertyName("title")] string Title,
    [property: JsonPropertyName("uri")] string Uri,
    [property: JsonPropertyName("postedBy")] string PostedBy,
    [property: JsonPropertyName("time")] string Time,
    [property: JsonPropertyName("score")] int Score,
    [property: JsonPropertyName("commentCount")] int CommentCount
);