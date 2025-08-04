using HtmlAgilityPack;
using UniversalNewsFeedApp.Model;
using System.Net;

namespace UniversalNewsFeedApp.Services
{
    public class UniversalNewsService : INewsService
    {
        private readonly SourceConfig _config;
        private readonly IHtmlLoader _htmlLoader;

        public UniversalNewsService(SourceConfig config, IHtmlLoader htmlLoader)
        {
            _config = config;
            _htmlLoader = htmlLoader;
        }

        public List<NewsArticle> FetchNews()
        {
            var articles = new List<NewsArticle>();
            var doc = _htmlLoader.Load(_config.PageUrl);
            var linkNodes = doc.DocumentNode.SelectNodes(_config.LinkSelector);
            if (linkNodes == null)
            {
                return articles;
            }

            foreach (var linkNode in linkNodes)
            {              
                var href = ExtractUrl(linkNode);
                if (string.IsNullOrEmpty(href))
                {
                    continue;
                }

                var fullUrl = href.StartsWith("/") ? _config.BaseUrl + href : href;
                var titleNode = linkNode.SelectSingleNode(_config.TitleSelector);
                var title = WebUtility.HtmlDecode(titleNode?.InnerText?.Trim() ?? "Untitled");

                if (string.IsNullOrEmpty(title) || articles.Any(a => a.Headline == title))
                {
                    continue;
                }

                articles.Add(new NewsArticle
                {
                    Source = _config.Source,
                    Headline = title,
                    Url = fullUrl,
                    DownloadedAt = DateTime.UtcNow
                });
            }
            return articles;
        }

        private string ExtractUrl(HtmlNode node)
        {
            if (string.IsNullOrWhiteSpace(_config.UrlSelector))
            {
                return "";
            }

            if (!(_config.UrlSelector.StartsWith("//") || _config.UrlSelector.Contains("/")))
            {
                return node.GetAttributeValue(_config.UrlSelector, "");
            }

            var urlNode = node.SelectSingleNode(_config.UrlSelector);

            return urlNode?.GetAttributeValue("href", "") ?? "";
        }
    }
}
