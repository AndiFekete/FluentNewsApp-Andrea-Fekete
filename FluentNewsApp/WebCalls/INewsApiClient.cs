using FluentNewsApp.Models;

namespace FluentNewsApp.WebCalls
{
    public interface INewsApiClient
    {
        Task<List<Article>> GetNewsByCategoryAsync(string category); 
    }
}
