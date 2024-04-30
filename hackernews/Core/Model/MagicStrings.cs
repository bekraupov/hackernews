namespace hackernews.Core.Model
{
    public static class MagicStrings
    {
        public const string HackerNewsHttpClientFactory = "HackerNews";

        public class CacheKeys
        {
            public const string BestStories = "HackerNews.BestStories";

            public static string ByStory(int id) => $"HackerNews.Story.{id}";
        }
    }
}
