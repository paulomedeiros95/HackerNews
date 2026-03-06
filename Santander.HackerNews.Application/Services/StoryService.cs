using Microsoft.Extensions.Caching.Memory;
using Santander.HackerNews.Domain.Dtos;
using Santander.HackerNews.Domain.Interfaces;

namespace Santander.HackerNews.Application.Services;

public class StoryService : IStoryService
{
    #region Fields
    private readonly IMemoryCache _cache;
    private const string CacheKey = "BestStoriesCache";
    #endregion

    #region Constructor

    public StoryService(IMemoryCache cache)
    {
        _cache = cache;
    }
    #endregion

    #region Methods

    public Task<IEnumerable<StoryDto>> GetBestStoriesAsync(int n, CancellationToken cancellationToken)
    {
        if (n <= 0)
        {
            return Task.FromResult(Enumerable.Empty<StoryDto>());
        }

        // Retrieves efficiently from cache, protecting external API 
        if (_cache.TryGetValue(CacheKey, out List<StoryDto>? cachedStories) && cachedStories != null)
        {
            return Task.FromResult(cachedStories.Take(n)); // Returns best n stories 
        }

        return Task.FromResult(Enumerable.Empty<StoryDto>());
    }
    #endregion
}