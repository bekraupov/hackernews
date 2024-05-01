namespace hackernews.Core.Config
{
    public class HackerNewsConfig
    {
        public const string Section = "HackerNews";
        
        public required string BaseUri { get; set; }
    }
}
