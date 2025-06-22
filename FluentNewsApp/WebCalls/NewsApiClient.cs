using FluentNewsApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace FluentNewsApp.WebCalls
{
    public sealed class NewsApiClient : INewsApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<List<Article>> GetNewsByCategoryAsync(string category)
        {
            var url = $"https://newsapi.org/v2/top-headlines?category={category}";
            var response = await GetAsync(url);
            var jsonString = await response.Content.ReadAsStringAsync();

            var jsonObject = JsonConvert.DeserializeObject<JObject>(jsonString);
            if (jsonObject == null)
            {
                throw new JsonException("Failed to deserialize JSON response.");
            }

            var articleObjects = jsonObject["articles"]?.ToObject<List<JObject>>();
            if (articleObjects == null)
            {
                return new List<Article>();
            }

            // Simulate network delay and errors

            //var rand = Random.Shared.Next(200, 1000);
            //await Task.Delay(rand);
            //if (rand < 400)
            //{
            //    throw new Exception("simulate error");
            //}

            return articleObjects.Select(article => new Article
            {
                Title = article["title"]?.ToString() ?? "Unknown Title",
                Published = DateTime.TryParse(article["publishedAt"]?.ToString(), out var publishedDate)
                    ? publishedDate
                    : DateTime.MinValue
            }).ToList();
        }

        private async Task<HttpResponseMessage> GetAsync(string url)
        {
            var client = _httpClientFactory.CreateClient("newsAPI");
            var response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                return response;
            }

            throw new HttpRequestException($"Error fetching news: {response.ReasonPhrase}");
        }
    }
}
