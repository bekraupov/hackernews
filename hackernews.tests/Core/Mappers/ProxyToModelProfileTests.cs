using AutoMapper;
using hackernews.Core.Mappers;
using hackernews.Core.Model;
using hackernews.Core.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.tests.Core.Mappers
{
    internal class ProxyToModelProfileTests
    {
        private MapperConfiguration _configuration;

        [SetUp]
        public void Setup()
        {

            _configuration = new MapperConfiguration(cfg =>
            {
                // Add all Profiles from the Assembly containing this Type
                cfg.AddMaps(typeof(ProxyToModelProfile));
            });

            
        }

        [Test]
        public async Task ProxyToModelWorks()
        {
            var mapper = _configuration.CreateMapper();
            
            var storyResponse = new StoryResponse {
                By = "throwup238",
                Descendants = 563,
                Score = 1007,
                Time = 1714065780,
                Title = "FCC votes to restore net neutrality rules",
                Url = "https://www.nytimes.com/2024/04/25/technology/fcc-net-neutrality-open-internet.html"

            };
            
            var model = mapper.Map<StoryModel>(storyResponse);

            Assert.AreEqual(563, model.CommentCount);
            Assert.AreEqual("throwup238", model.PostedBy);
            Assert.AreEqual(1007, model.Score);
            Assert.AreEqual("25/04/2024 17:23:00", model.Time.ToString());
            Assert.AreEqual("FCC votes to restore net neutrality rules", model.Title);
            Assert.AreEqual("https://www.nytimes.com/2024/04/25/technology/fcc-net-neutrality-open-internet.html", model.Url);
        }
    }
}