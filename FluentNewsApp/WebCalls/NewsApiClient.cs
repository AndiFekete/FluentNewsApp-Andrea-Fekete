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

            var jsonOnbject = JsonConvert.DeserializeObject<JObject>(jsonString);
            //TODO handle status != ok
            var articleObjects = jsonOnbject["articles"]?.ToObject<List<JObject>>();

            // Simulate network delay and errors

            //var rand = Random.Shared.Next(200, 1000);
            //await Task.Delay(rand);
            //if (rand < 400)
            //{
            //    throw new Exception("simulate error");
            //}


            return articleObjects?.Select(article => new Article
            {
                Title = article["title"].ToString(),
                Published = DateTime.Parse(article["publishedAt"]?.ToString())
            }).ToList() ?? new List<Article>();
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
