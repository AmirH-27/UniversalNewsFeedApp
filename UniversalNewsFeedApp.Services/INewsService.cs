using UniversalNewsFeedApp.Model;
namespace UniversalNewsFeedApp.Services
{
    public interface INewsService
    {
        List<NewsArticle> FetchNews();
    }
}
