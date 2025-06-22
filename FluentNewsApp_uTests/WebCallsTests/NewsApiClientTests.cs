using FluentNewsApp.Models;
using FluentNewsApp.WebCalls;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework.Legacy;
using System.Globalization;
using System.Net;
using System.Net.Http.Json;
using System.Security.Policy;

namespace FluentNewsApp_uTests.WebCallsTests
{
    public class NewsApiClientTests
    {
        private static readonly Article[] _testArticles =
        [
            new Article { Title = "Tyrese Haliburton plays through injury, sparks Pacers to force Game 7 - The Washington Post", Published = DateTime.Parse("2025-06-20T05:48:50Z") },
            new Article { Title = "Trump can keep National Guard in Los Angeles, appeals court rules - The Washington Post", Published = DateTime.Parse("2025-06-20T04:20:19Z") }
        ];

        [Test]
        public async Task GetNewsByCategory_ReturnsArticles()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        @"{
                          ""status"": ""ok"",
                          ""totalResults"": 36,
                          ""articles"": [
                            {
                              ""source"": {
                                ""id"": ""the-washington-post"",
                                ""name"": ""The Washington Post""
                              },
                              ""author"": ""Ben Golliver"",
                              ""title"": ""Tyrese Haliburton plays through injury, sparks Pacers to force Game 7 - The Washington Post"",
                              ""description"": ""Guard scores 14 points in 23 minutes as Indiana rolls at home, 108-91, to send the NBA Finals to a winner-take-all finale in Oklahoma City on Sunday night."",
                              ""url"": ""https://www.washingtonpost.com/sports/2025/06/19/pacers-thunder-nba-finals-tyrese-haliburton/"",
                              ""urlToImage"": ""https://www.washingtonpost.com/wp-apps/imrs.php?src=https://arc-anglerfish-washpost-prod-washpost.s3.amazonaws.com/public/AJHUYW3EFUFPCLFHMJLGSRAKKE.jpg&w=1440"",
                              ""publishedAt"": ""2025-06-20T07:48:50Z"",
                              ""content"": ""INDIANAPOLIS For Indiana Pacers guard Tyrese Haliburton, one good leg was more than enough to deny the Oklahoma City Thunder its first chance to win the NBA championship.\r\nHaliburton gutted through a… [+6917 chars]""
                            },
                            {
                              ""source"": {
                                ""id"": ""the-washington-post"",
                                ""name"": ""The Washington Post""
                              },
                              ""author"": ""Perry Stein"",
                              ""title"": ""Trump can keep National Guard in Los Angeles, appeals court rules - The Washington Post"",
                              ""description"": ""The federal appeals court’s ruling was a win for President Donald Trump as he aims to use the military to police protests of his deportation efforts."",
                              ""url"": ""https://www.washingtonpost.com/politics/2025/06/20/trump-keep-national-guard-la-ruling/"",
                              ""urlToImage"": ""https://www.washingtonpost.com/wp-apps/imrs.php?src=https://arc-anglerfish-washpost-prod-washpost.s3.amazonaws.com/public/THKJRUZBE3SRLIMQ7456BLFY3E_size-normalized.JPG&w=1440"",
                              ""publishedAt"": ""2025-06-20T06:20:19Z"",
                              ""content"": ""A federal appeals court in San Francisco said Thursday that President Donald Trump can keep the California National Guard in Los Angeles for now, delivering a win for the president as he aims to use … [+4200 chars]""
                            }
                          ]
                        }")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var _newsApiClient = new NewsApiClient(httpClientFactoryMock.Object);

            var articles = await _newsApiClient.GetNewsByCategoryAsync("business");

            Assert.That(articles.Count, Is.EqualTo(2));
            Assert.That(articles[0].Title, Is.EqualTo(_testArticles[0].Title));
            Assert.That(articles[0].Published, Is.EqualTo(_testArticles[0].Published));
            Assert.That(articles[1].Title, Is.EqualTo(_testArticles[1].Title));
            Assert.That(articles[1].Published, Is.EqualTo(_testArticles[1].Published));
        }

        [Test]
        public async Task GetNewsByCategory_IfNoArticles_ReturnsEmptyList()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        @"{
                          ""status"": ""ok"",
                          ""totalResults"": 0,
                          ""articles"": [
                          ]
                        }")
                });

            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);

            var _newsApiClient = new NewsApiClient(httpClientFactoryMock.Object);

            var articles = await _newsApiClient.GetNewsByCategoryAsync("business");

            Assert.That(articles.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetNewsByCategory_IfApiError_ThrowsException()
        {
            var handlerMock = new Mock<HttpMessageHandler>();
            handlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage()
                {
                    StatusCode = HttpStatusCode.InternalServerError,
                    Content = new StringContent("Internal Server Error")
                });
            var httpClient = new HttpClient(handlerMock.Object);
            var httpClientFactoryMock = new Mock<IHttpClientFactory>();
            httpClientFactoryMock
                .Setup(x => x.CreateClient(It.IsAny<string>()))
                .Returns(httpClient);
            var _newsApiClient = new NewsApiClient(httpClientFactoryMock.Object);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _newsApiClient.GetNewsByCategoryAsync("business"));
        }

    [Test]
    public void GetNewsByCategory_IfJsonMalformed_ThrowsException()
    {
        var handlerMock = new Mock<HttpMessageHandler>();
        handlerMock
            .Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage()
            {
                StatusCode = HttpStatusCode.OK,
                Content = new StringContent("fsafsasfaf")
            });
        var httpClient = new HttpClient(handlerMock.Object);
        var httpClientFactoryMock = new Mock<IHttpClientFactory>();
        httpClientFactoryMock
            .Setup(x => x.CreateClient(It.IsAny<string>()))
            .Returns(httpClient);
        var _newsApiClient = new NewsApiClient(httpClientFactoryMock.Object);

        Assert.ThrowsAsync<JsonReaderException>(async () => await _newsApiClient.GetNewsByCategoryAsync("business"));
    }
    }
}
