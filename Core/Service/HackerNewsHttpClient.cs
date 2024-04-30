using hackernews.Core.Model;
using hackernews.Core.Proxy;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;

namespace hackernews.Core.Service
{
    /// <summary>
    /// interface for HackerNews API as per: https://github.com/HackerNews/API?tab=readme-ov-file
    /// </summary>
    public interface IHackerNewsClient
    {
        /// <summary>
        /// Returns array of BestStories (top 200)
        /// </summary>
        /// <returns></returns>
        Task<int[]> GetBestStoryIds();

        Task<StoryResponse> GetStory(int storyId);
    }

    /// <summary>
    /// Http implementation of the client
    /// </summary>
    public class HackerNewsHttpClient : IHackerNewsClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public HackerNewsHttpClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
        }

        public async Task<int[]> GetBestStoryIds()
        {
            var httpClient = _httpClientFactory.CreateClient(MagicStrings.HackerNewsHttpClientFactory);
            return await httpClient.GetFromJsonAsync<int[]>("beststories.json");
        }

        public async Task<StoryResponse> GetStory(int storyId)
        {
            var httpClient = _httpClientFactory.CreateClient(MagicStrings.HackerNewsHttpClientFactory);
            return await httpClient.GetFromJsonAsync<StoryResponse>($"item/{storyId}.json");
        }
    }
}
