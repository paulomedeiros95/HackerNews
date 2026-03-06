using System.Text.Json.Serialization;

namespace Santander.HackerNews.Domain.Entities
{
    /// <summary>
    /// Represents the exact JSON structure returned by the Hacker News API.
    /// </summary>
    public class HackerNewsStory
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("url")]
        public string Url { get; set; } = string.Empty;

        [JsonPropertyName("by")]
        public string By { get; set; } = string.Empty;

        [JsonPropertyName("time")]
        public long Time { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("descendants")]
        public int Descendants { get; set; }
    }
}
