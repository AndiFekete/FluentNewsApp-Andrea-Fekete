using FluentNewsApp.Commands;
using FluentNewsApp.Models;
using FluentNewsApp.WebCalls;
using System.Windows.Input;

namespace FluentNewsApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly NewsApiClient _newsApiClient;
        private ArticleFeed _technologyNews;
        private ArticleFeed _healthNews;
        private ArticleFeed _entertainmentNews;

        public ArticleFeed TechnologyNews
        {
            get
            {
                return _technologyNews;
            }
            set
            {
                _technologyNews = value;
                OnPropertyChanged();
            }
        }
        public ArticleFeed HealthNews
        {
            get
            {
                return _healthNews;
            }
            set
            {
                _healthNews = value;
                OnPropertyChanged();
            }
        }
        public ArticleFeed EntertainmentNews
        {
            get
            {
                return _entertainmentNews;
            }
            set
            {
                _entertainmentNews = value;
                OnPropertyChanged();
            }
        }

        public ICommand RefreshCommand { get; }

        public MainWindowViewModel(NewsApiClient newsApiClient)
        {
            _newsApiClient = newsApiClient ?? throw new ArgumentNullException(nameof(newsApiClient));

            TechnologyNews = new ArticleFeed { Category = "Technology", Articles = new List<Article>() };
            HealthNews = new ArticleFeed { Category = "Health", Articles = new List<Article>() };
            EntertainmentNews = new ArticleFeed { Category = "Entertainment", Articles = new List<Article>() };

            RefreshCommand = new RelayCommand(RefreshFeeds, ex => Console.WriteLine($"Error refreshing feeds: {ex.Message}"));  //TODO replace placeholder exception handling
        }

        private async Task RefreshFeeds()
        {
            //test
            var tech = await _newsApiClient.GetNewsByCategoryAsync("technology");
            TechnologyNews = new ArticleFeed { Category = "Technology", Articles = tech, HasError = true };
            var health = await _newsApiClient.GetNewsByCategoryAsync("health");
            HealthNews = new ArticleFeed { Category = "Health", Articles = health };
        }
    }
}
