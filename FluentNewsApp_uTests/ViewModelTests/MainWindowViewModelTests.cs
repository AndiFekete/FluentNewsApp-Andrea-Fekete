using FluentNewsApp.Models;
using FluentNewsApp.ViewModels;
using FluentNewsApp.WebCalls;
using Moq;

namespace FluentNewsApp_uTests.ViewModelTests
{
    public class MainWindowViewModelTests
    {
        private readonly List<Article> _testArticles =
            new List<Article>
            {
                new Article { Title = "News 1", Published = DateTime.UtcNow },
                new Article { Title = "News 2", Published = DateTime.UtcNow }
            };
        private Mock<INewsApiClient> _newsApiClientMock;
        private MainWindowViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _newsApiClientMock = new Mock<INewsApiClient>();
            _newsApiClientMock.Setup(x => x.GetNewsByCategoryAsync(It.IsAny<string>()))
                .ReturnsAsync(_testArticles);
            _viewModel = new MainWindowViewModel(_newsApiClientMock.Object);
        }

        [Test]
        public void Constructor_SetsInitialState()
        {
            Assert.That(_viewModel.TechnologyNews, Is.Not.Null);
            Assert.That(_viewModel.TechnologyNews.Category, Is.EqualTo("Technology"));
            Assert.That(_viewModel.HealthNews, Is.Not.Null);
            Assert.That(_viewModel.HealthNews.Category, Is.EqualTo("Health"));
            Assert.That(_viewModel.EntertainmentNews, Is.Not.Null);
            Assert.That(_viewModel.EntertainmentNews.Category, Is.EqualTo("Entertainment"));
        }

        [Test]
        public void Constructor_NewsApiClientIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => new MainWindowViewModel(null!));
        }

        [Test]
        public async Task RefreshFeeds_CallsNewsApiClient()
        {
            // Act
            await _viewModel.RefreshFeeds();
            // Assert
            _newsApiClientMock.Verify(x => x.GetNewsByCategoryAsync("technology"), Times.Once);
            _newsApiClientMock.Verify(x => x.GetNewsByCategoryAsync("health"), Times.Once);
            _newsApiClientMock.Verify(x => x.GetNewsByCategoryAsync("entertainment"), Times.Once);
        }

        [Test]
        public async Task RefreshFeeds_UpdatesAllNews()
        {
            // Act
            await _viewModel.RefreshFeeds();

            // Assert
            Assert.That(_viewModel.TechnologyNews.Articles.Count, Is.EqualTo(2));
            Assert.That(_viewModel.HealthNews.Articles.Count, Is.EqualTo(2));
            Assert.That(_viewModel.EntertainmentNews.Articles.Count, Is.EqualTo(2));

            Assert.That(_viewModel.TechnologyNews.HasError, Is.False);
            Assert.That(_viewModel.HealthNews.HasError, Is.False);
            Assert.That(_viewModel.EntertainmentNews.HasError, Is.False);

            Assert.That(_viewModel.TechnologyNews.Articles[0].Title, Is.EqualTo(_testArticles[0].Title));
            Assert.That(_viewModel.TechnologyNews.Articles[1].Title, Is.EqualTo(_testArticles[1].Title));

            Assert.That(_viewModel.TechnologyNews.Articles[0].Published, Is.EqualTo(_testArticles[0].Published));
            Assert.That(_viewModel.TechnologyNews.Articles[1].Published, Is.EqualTo(_testArticles[1].Published));
        }

        [Test]
        public async Task RefreshFeeds_WhenApiCallFails_SetsHasErrorToTrue()
        {
            // Arrange
            _newsApiClientMock.Setup(x => x.GetNewsByCategoryAsync(It.IsAny<string>()))
                .ThrowsAsync(new Exception("API call failed"));
            // Act
            await _viewModel.RefreshFeeds();
            // Assert
            Assert.That(_viewModel.TechnologyNews.HasError, Is.True);
            Assert.That(_viewModel.HealthNews.HasError, Is.True);
            Assert.That(_viewModel.EntertainmentNews.HasError, Is.True);
        }
    }
}
