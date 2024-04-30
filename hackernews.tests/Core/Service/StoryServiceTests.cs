using AutoMapper;
using hackernews.Core.Mappers;
using hackernews.Core.Model;
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
    /// <summary>
    /// unit tests
    /// </summary>
    internal class StoryServiceTests
    {
        [Test]
        public async Task GetTopBestStoriesWhenAllInCache()
        {
            //Arrange

            //want to make sure we dont call to outside if all from cache
            var client = new Mock<IHackerNewsClient>(MockBehavior.Strict);
            var mockLogger = new Mock<ILogger<StoryService>>();
                        
            var cache = new Mock<IMemoryCache>();
            int[] idArray = [1, 2, 3];
            object ids = idArray as object;

            cache.Setup(x => x.TryGetValue(MagicStrings.CacheKeys.BestStories, out ids))
                    .Returns(true);

            
            object story1 = new StoryModel { Title = "1", Score = 10 };
            object story2 = new StoryModel { Title = "2", Score = 5 };
            object story3 = new StoryModel { Title = "3", Score = 20 };

            cache.Setup(x => x.TryGetValue(MagicStrings.CacheKeys.ByStory(1), out story1))
                    .Returns(true);
            cache.Setup(x => x.TryGetValue(MagicStrings.CacheKeys.ByStory(2), out story2))
                    .Returns(true);
            cache.Setup(x => x.TryGetValue(MagicStrings.CacheKeys.ByStory(3), out story3))
                    .Returns(true);

            var configuration = new MapperConfiguration(cfg =>
            {
                // Add all Profiles from the Assembly containing this Type
                cfg.AddMaps(typeof(ProxyToModelProfile));
            });
            var mapper = configuration.CreateMapper();

            //Act
            var service = new StoryService(mockLogger.Object, cache.Object, client.Object, mapper);

            var models = (await service.GetTopBestStories(2)).ToList();

            //Assert

            Assert.AreEqual(2, models.Count);

            //we expect only store 1 and 3 being returned as we asked for top 2
            Assert.AreEqual("3", models[0].Title);
            Assert.AreEqual(20, models[0].Score);

            Assert.AreEqual("1", models[1].Title);
            Assert.AreEqual(10, models[1].Score);


            cache.Verify();
            client.Verify();
        }

    }
}

