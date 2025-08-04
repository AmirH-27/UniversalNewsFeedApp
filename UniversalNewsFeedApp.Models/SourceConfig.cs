namespace UniversalNewsFeedApp.Model
{
 
    public class SourceConfig
    {       
        public string Source { get; set; }    
        public string BaseUrl { get; set; }      
        public string PageUrl { get; set; }    
        public string LinkSelector { get; set; }
        public string TitleSelector { get; set; }
        public string UrlSelector { get; set; }
    }
}