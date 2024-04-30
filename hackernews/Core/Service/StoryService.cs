using AutoMapper;
using hackernews.Core.Model;
using Microsoft.Extensions.Caching.Memory;
using System.Data.Common;
using System.Threading;

namespace hackernews.Core.Service
{
    public interface IStoryService
    {
        /// <summary>
        /// REturns Best Stories (ordered by Score) by given TopX
        /// </summary>
        /// <param name="topCount">number of top stories to return (i.e. if 10, will return top 10 stories ordered by Score)</param>
        /// <returns></returns>
        Task<IEnumerable<StoryModel>> GetTopBestStories(int topCount);
    }

    public class StoryService : IStoryService
    {
        private ILogger<StoryService> _logger;
        //TODO: in mem cache, later on switch to Redis or equi (make sure you have correct rolling/retension policy)
        private IMemoryCache _memoryCache;
        private IHackerNewsClient _hackerNewsClient;
        private IMapper _mapper;
        private readonly SemaphoreSlim _lock = new(1);

        private readonly MemoryCacheEntryOptions _cachePolicyStory = new()
        {
            SlidingExpiration = TimeSpan.FromMinutes(10),
        };
                
        public StoryService(ILogger<StoryService> logger, IMemoryCache memoryCache, IHackerNewsClient hackerNewsClient, IMapper mapper)
        {
            _logger = logger;
            _memoryCache = memoryCache;
            _hackerNewsClient = hackerNewsClient;
            _mapper = mapper;
        }

        public async Task<IEnumerable<StoryModel>> GetTopBestStories(int topCount)
        {
            var result = new List<StoryModel>();
            var bestStoriesIds = await GetBestStoriesIds();
            var storiesData = await GetStoriesData(bestStoriesIds);

            foreach (var storyId in bestStoriesIds)
            {
                result.Add(storiesData[storyId]);
            }

            return result.OrderByDescending(x => x.Score).Take(topCount);
        }


        /// <summary>
        /// retrieve from cache if available otherwise get from proxy, populate cache and return
        /// </summary>
        /// <returns></returns>
        private async Task<int[]> GetBestStoriesIds()
        {
            if (_memoryCache.TryGetValue(MagicStrings.CacheKeys.BestStories, out int[] bestStoriesIds))
            {
                _logger.LogDebug($"Retrieving BestStories from Cache");
                return bestStoriesIds;
            }

            await _lock.WaitAsync();

            try
            {
                bestStoriesIds = await _hackerNewsClient.GetBestStoryIds();
                _memoryCache.Set(MagicStrings.CacheKeys.BestStories, bestStoriesIds, _cachePolicyStory);

                return bestStoriesIds;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }

        private async Task<Dictionary<int, StoryModel>> GetStoriesData(int[] storyIds)
        {
            var storyModelByIdMap = new Dictionary<int, StoryModel>();

            await _lock.WaitAsync();
            try
            {
                foreach (var id in storyIds)
                {
                    var key = MagicStrings.CacheKeys.ByStory(id);
                    if (!_memoryCache.TryGetValue(key, out StoryModel model))
                    {
                        var storyResponse = await _hackerNewsClient.GetStory(id);
                        model = _mapper.Map<StoryModel>(storyResponse);
                        _memoryCache.Set(key, model, _cachePolicyStory);
                    }

                    storyModelByIdMap.Add(id, model);
                }

                return storyModelByIdMap;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }
    }
}
