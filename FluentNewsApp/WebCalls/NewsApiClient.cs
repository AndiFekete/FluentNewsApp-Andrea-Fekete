using System.Net.Http;

namespace FluentNewsApp.WebCalls
{
    public sealed class NewsApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public NewsApiClient(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> GetTopNewsAsync()
        {
            var url = "https://newsapi.org/v2/top-headlines?country=us";
            return await GetAsync(url).ContinueWith(response => response.Result.Content.ReadAsStringAsync().Result);
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
