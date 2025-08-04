using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalNewsFeedApp.Services
{
    public interface IHtmlLoader
    {
        Task<HtmlDocument> LoadAsync(string url);
    }
}
