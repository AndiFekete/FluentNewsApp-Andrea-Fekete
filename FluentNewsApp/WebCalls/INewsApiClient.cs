using FluentNewsApp.Models;

namespace FluentNewsApp.WebCalls
{
    interface INewsApiClient
    {
        Task<List<Article>> GetNewsByCategoryAsync(string category); 
    }
}
