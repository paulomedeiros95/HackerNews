using System.Net.Http.Json;
using Santander.HackerNews.Domain.Entities;
using Santander.HackerNews.Domain.Interfaces;

namespace Santander.HackerNews.Infrastructure.Clients
{
    public class HackerNewsClient : IHackerNewsClient
    {
        #region Fields
        private readonly HttpClient _httpClient;
        #endregion

        #region Constructor

        public HackerNewsClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/"); 
        }
        #endregion

        #region Methods

        public async Task<IEnumerable<int>> GetBestStoryIdsAsync(CancellationToken cancellationToken)
        {
           // Retrieves IDs from the beststories endpoint 
            var response = await _httpClient.GetFromJsonAsync<IEnumerable<int>>("beststories.json", cancellationToken);
            return response ?? Enumerable.Empty<int>();
        }

        public async Task<HackerNewsStory?> GetStoryByIdAsync(int id, CancellationToken cancellationToken)
        {
            // Retrieves individual story details 
            return await _httpClient.GetFromJsonAsync<HackerNewsStory>($"item/{id}.json", cancellationToken);
        }
        #endregion
    }
}
