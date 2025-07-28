using HtmlAgilityPack;
using UniversalNewsFeedApp.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace UniversalNewsFeedApp.Services
{
    // Service for fetching news articles
    public class UniversalNewsService : INewsService
    {
        private readonly SourceConfig _config;

        // Initializes the service with a source configuration
        public UniversalNewsService(SourceConfig config)
        {
            _config = config;
        }

        // Fetches news articles
        public List<NewsArticle> FetchNews()
        {
            var articles = new List<NewsArticle>();

            // Load the HTML document from the page URL
            var web = new HtmlWeb();
            var doc = web.Load(_config.PageUrl);

            // Select all article link nodes using the configured selector
            var linkNodes = doc.DocumentNode.SelectNodes(_config.LinkSelector);
            if (linkNodes == null) return articles;

            // Iterate through each link node to extract article details
            foreach (var linkNode in linkNodes)
            {
                // Extract the URL from the link node
                string href = ExtractUrl(linkNode);
                if (string.IsNullOrEmpty(href)) continue;

                // Build the full URL (handle relative URLs)
                var fullUrl = href.StartsWith("/") ? _config.BaseUrl + href : href;

                // Extract the article title using the configured selector
                var titleNode = linkNode.SelectSingleNode(_config.TitleSelector);
                var title = WebUtility.HtmlDecode(titleNode?.InnerText?.Trim() ?? "Untitled");

                // Add the article if the title is valid and not already in the list
                if (!string.IsNullOrEmpty(title) && !articles.Any(a => a.Headline == title))
                {
                    articles.Add(new NewsArticle
                    {
                        Source = _config.Source,
                        Headline = title,
                        Url = fullUrl,
                        DownloadedAt = DateTime.Now
                    });
                }
            }

            // Return the list of fetched articles
            return articles;
        }

        // Extracts the article URL from the HTML node using the configured selector
        private string ExtractUrl(HtmlNode node)
        {
            if (string.IsNullOrWhiteSpace(_config.UrlSelector)) return "";

            // If the UrlSelector is a simple attribute like "href"
            if (!(_config.UrlSelector.StartsWith("//") || _config.UrlSelector.Contains("/")))
            {
                return node.GetAttributeValue(_config.UrlSelector, "");
            }

            // Otherwise, treat it as an XPath expression to locate a child node
            var urlNode = node.SelectSingleNode(_config.UrlSelector);
            return urlNode?.GetAttributeValue("href", "") ?? "";
        }
    }
}
