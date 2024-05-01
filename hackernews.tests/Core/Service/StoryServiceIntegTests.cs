using AutoMapper;
using hackernews.Core.Mappers;
using hackernews.Core.Service;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.tests.Core.Service
{
    internal class StoryServiceIntegTests
    {
        private IStoryService _service;

        [SetUp]
        public void Setup()
        {
            var client = new HackerNewsHttpClient(new DefaultHttpClientFactory());
            var mockLogger= new Mock<ILogger<StoryService>>();
            var cache = new MemoryCache(new MemoryCacheOptions());

            var configuration = new MapperConfiguration(cfg =>
            {
                // Add all Profiles from the Assembly containing this Type
                cfg.AddMaps(typeof(ProxyToModelProfile));
            });
            var mapper = configuration.CreateMapper();


            _service = new StoryService(mockLogger.Object, cache, client, mapper);
        }

        [Test]
        public async Task GetTopBestStoriesWorks()
        {
            var models = (await _service.GetTopBestStories(5)).ToList();

            Assert.AreEqual(5, models.Count);
            //make sure it is ordered
            Assert.That(models[0].Score > models[1].Score);
        }
    }
}
