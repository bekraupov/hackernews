using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace hackernews.tests
{
    public sealed class DefaultHttpClientFactory : IHttpClientFactory, IDisposable
    {
        private readonly Lazy<HttpMessageHandler> _handlerLazy = new(() => new HttpClientHandler());

        public HttpClient CreateClient(string name)
        {
            //TODO: hardcoding for now for integration test (we can pick up from config later on)
            return new(_handlerLazy.Value, disposeHandler: false)
            {
                BaseAddress = new Uri("https://hacker-news.firebaseio.com/v0/")
            };
        }

        public void Dispose()
        {
            if (_handlerLazy.IsValueCreated)
            {
                _handlerLazy.Value.Dispose();
            }
        }
    }


    
}
