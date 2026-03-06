namespace Santander.HackerNews.Domain.Interfaces
{
    /// <summary>
    /// Contract for the application use case.
    /// </summary>
    public interface IStoryService
    {
        Task<IEnumerable<Dtos.StoryDto>> GetBestStoriesAsync(int n, CancellationToken cancellationToken);
    }
}
