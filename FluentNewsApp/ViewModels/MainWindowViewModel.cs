using FluentNewsApp.Models;

namespace FluentNewsApp.ViewModels
{
    public class MainWindowViewModel
    {
        public ArticleFeed TechnologyNews { get; set; }
        public ArticleFeed HealthNews { get; set; }
        public ArticleFeed EntertainmentNews { get; set; }

        public MainWindowViewModel()
        {
            // Initialize with some sample data
            TechnologyNews =
                new ArticleFeed
                {
                    Category = "Technology",
                    Articles = new List<Article>
                    {
                        new Article { Title = "Tech News 1", Published = DateTime.Now.AddDays(-1) },
                        new Article { Title = "Tech News 2", Published = DateTime.Now.AddDays(-2) }
                    }
                };
            HealthNews =
                new ArticleFeed
                {
                    Category = "Health",
                    Articles = new List<Article>
                    {
                        new Article { Title = "Health News 1", Published = DateTime.Now.AddDays(-3) },
                        new Article { Title = "Health News 2", Published = DateTime.Now.AddDays(-4) }
                    }
                };
            EntertainmentNews =
                new ArticleFeed
                {
                    Category = "Entertainment",
                    Articles = new List<Article>
                    {
                        new Article { Title = "Entertainment News 1", Published = DateTime.Now.AddDays(-3) },
                        new Article { Title = "Entertainment News 2", Published = DateTime.Now.AddDays(-4) }
                    }
                };
        }
    }
}
