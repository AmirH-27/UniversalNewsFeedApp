namespace UniversalNewsFeedApp.Services
{
    // Represents configuration for a news source
    public class SourceConfig
    {
        // Name of the news source
        public string Source { get; set; }
        // Base URL of the news website
        public string BaseUrl { get; set; }
        // URL pattern for the news page
        public string PageUrl { get; set; }
        // CSS selector for finding article links
        public string LinkSelector { get; set; }
        // CSS selector for extracting article titles
        public string TitleSelector { get; set; }
        // CSS selector for extracting article URLs
        public string UrlSelector { get; set; }
    }
}