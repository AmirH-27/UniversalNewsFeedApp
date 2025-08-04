using HtmlAgilityPack;

namespace UniversalNewsFeedApp.Services;

public class HtmlLoader : IHtmlLoader
{
    public HtmlDocument Load(string url)
    {
        var web = new HtmlWeb();
        return web.Load(url);
    }
}
