using HtmlAgilityPack;
using UniversalNewsFeedApp.Model;
using System.Net;
using System.Reflection;

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

        public async IAsyncEnumerable<NewsArticle> FetchNewsAsync()
        {
            var articles = new List<NewsArticle>();
            var doc = await _htmlLoader.LoadAsync(_config.PageUrl);
            var linkNodes = doc.DocumentNode.SelectNodes(_config.LinkSelector);
            if (linkNodes == null)
            {
                yield break;
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

                yield return new NewsArticle
                {
                    Source = _config.Source,
                    Headline = title,
                    Url = fullUrl,
                    DownloadedAt = DateTime.UtcNow
                };
            }
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
