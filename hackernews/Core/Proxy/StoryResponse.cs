namespace hackernews.Core.Proxy
{
    /// <summary>
    /// Contract we get from HackerNews
    /// </summary>
    public class StoryResponse
    {
        public string By { get; set; }

        public int Descendants { get; set; }

        public int Id { get; set; }

        public int Score { get; set; }

        public long Time { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
        
    }
}
