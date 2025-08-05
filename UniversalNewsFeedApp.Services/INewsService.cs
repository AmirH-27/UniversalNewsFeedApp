using UniversalNewsFeedApp.Model;
namespace UniversalNewsFeedApp.Services
{
    public interface INewsService
    {
        IAsyncEnumerable<NewsArticle> FetchNewsAsync();
    }
}
