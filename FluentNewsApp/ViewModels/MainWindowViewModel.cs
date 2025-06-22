using FluentNewsApp.Commands;
using FluentNewsApp.Models;
using FluentNewsApp.WebCalls;
using System.Windows.Input;

namespace FluentNewsApp.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private readonly INewsApiClient _newsApiClient;
        //point for improvement: use a collection to store feeds by category
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

        public MainWindowViewModel(INewsApiClient newsApiClient)
        {
            _newsApiClient = newsApiClient ?? throw new ArgumentNullException(nameof(newsApiClient));

            TechnologyNews = new ArticleFeed { Category = "Technology", Articles = new List<Article>() };
            HealthNews = new ArticleFeed { Category = "Health", Articles = new List<Article>() };
            EntertainmentNews = new ArticleFeed { Category = "Entertainment", Articles = new List<Article>() };

            RefreshCommand = new RelayCommand(RefreshFeeds, ex => Console.WriteLine($"Error refreshing feeds: {ex.Message}"));  //TODO replace placeholder exception handling
        }

        internal async Task RefreshFeeds()
        {
            var technologyTask = RefreshTechnologyFeed();
            var healthTask = RefreshHealthFeed();
            var entertainmentTask = RefreshEntertainmentFeed();

            await Task.WhenAll(technologyTask, healthTask, entertainmentTask);
        }

        private async Task RefreshTechnologyFeed()
        {
            var technology = new List<Article>();
            TechnologyNews.IsLoading = true;
            try
            {
                technology = await _newsApiClient.GetNewsByCategoryAsync("technology");
            }
            catch (Exception)
            {
                TechnologyNews.HasError = true;
                TechnologyNews.IsLoading = false;
                return;
            }

            TechnologyNews.Articles = technology;
            TechnologyNews.HasError = false;
            TechnologyNews.IsLoading = false;
        }

        private async Task RefreshHealthFeed()
        {
            var health = new List<Article>();
            HealthNews.IsLoading = true;
            try
            {
                health = await _newsApiClient.GetNewsByCategoryAsync("health");
            }
            catch (Exception)
            {
                HealthNews.HasError = true;
                HealthNews.IsLoading = false;
                return;
            }

            HealthNews.Articles = health;
            HealthNews.HasError = false;
            HealthNews.IsLoading = false;
        }

        private async Task RefreshEntertainmentFeed()
        {
            var entertainment = new List<Article>();
            EntertainmentNews.IsLoading = true;
            try
            {
                entertainment = await _newsApiClient.GetNewsByCategoryAsync("entertainment");
            }
            catch (Exception)
            {
                EntertainmentNews.HasError = true;
                EntertainmentNews.IsLoading = false;
                return;
            }

            EntertainmentNews.Articles = entertainment;
            EntertainmentNews.HasError = false;
            EntertainmentNews.IsLoading = false;
        }
    }
}
