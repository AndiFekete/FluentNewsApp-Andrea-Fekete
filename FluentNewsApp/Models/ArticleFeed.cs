namespace FluentNewsApp.Models
{
    public sealed class ArticleFeed
    {
        public string Category { get; set; }
        public List<Article> Articles { get; set; }
    }
}
