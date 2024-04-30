using hackernews.Core.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.tests.Core.Service
{
    internal class HackerNewsHttpClientIntegTests
    {
        IHackerNewsClient _client;

        [SetUp]
        public void Setup()
        {
            _client = new HackerNewsHttpClient(new DefaultHttpClientFactory());
        }

        [Test]
        public async Task WhenGetBestStoryIdsCalledItWorksAsync()
        {
            var result = await _client.GetBestStoryIds();
            Assert.IsNotEmpty(result);
        }


        [Test]
        public async Task WhenValidGetStoryIdIsPassedItWorks()
        {
            var result = await _client.GetStory(40160429);
            Assert.IsNotNull(result);

            //example output: https://hacker-news.firebaseio.com/v0/item/40160429.json?print=pretty to compare with 

            Assert.AreEqual(40160429, result.Id);
            Assert.AreEqual("FCC votes to restore net neutrality rules", result.Title);
        }


    }
}
