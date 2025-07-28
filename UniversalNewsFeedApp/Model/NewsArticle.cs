using System;
using System.Diagnostics;

namespace UniversalNewsFeedApp.Model
{
    // Represents a news article
    public class NewsArticle
    {
        public string Source { get; set; }
        public string Headline { get; set; }
        public string Url { get; set; }
        public DateTime DownloadedAt { get; set; }

        // Opens the article URL in the default browser if not null
        public void OpenUrl()
        {
            if (!string.IsNullOrWhiteSpace(Url))
            {
                Process.Start(new ProcessStartInfo(Url) { UseShellExecute = true });
            }
        }
    }
}