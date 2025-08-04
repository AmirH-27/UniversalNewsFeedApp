using HtmlAgilityPack;
using System.Threading.Tasks;

namespace UniversalNewsFeedApp.Services;

public class HtmlLoader : IHtmlLoader
{
    private readonly HttpClient _httpClient;

    public HtmlLoader(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<HtmlDocument> LoadAsync(string url)
    {
        var html = await _httpClient.GetStringAsync(url);
        var doc = new HtmlDocument();
        doc.LoadHtml(html);
        return doc;
    }
}

