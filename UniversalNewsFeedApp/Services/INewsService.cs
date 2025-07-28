using UniversalNewsFeedApp.Model;
using System.Collections.Generic;

namespace UniversalNewsFeedApp.Services
{
    // Interface for fetching news articles
    public interface INewsService
    {
        // Fetches a list of news articles
        List<NewsArticle> FetchNews();
    }
}