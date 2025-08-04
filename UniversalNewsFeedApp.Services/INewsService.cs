using UniversalNewsFeedApp.Model;
namespace UniversalNewsFeedApp.Services
{
    public interface INewsService
    {
        Task<List<NewsArticle>> FetchNews();
    }
}
