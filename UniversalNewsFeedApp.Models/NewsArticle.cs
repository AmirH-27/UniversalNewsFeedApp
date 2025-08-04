using System;
using System.Diagnostics;

namespace UniversalNewsFeedApp.Model
{
    public class NewsArticle
    {
        public string Source { get; set; }
        public string Headline { get; set; }
        public string Url { get; set; }
        public DateTime DownloadedAt { get; set; }
    }
}