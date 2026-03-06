namespace Santander.HackerNews.Domain.Interfaces
{
    /// <summary>
    /// Contract for the external API integration.
    /// </summary>
    public interface IHackerNewsClient
    {
        Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken);
        Task<Entities.HackerNewsStory?> GetStoryByIdAsync(int id, CancellationToken cancellationToken);
    }
}
