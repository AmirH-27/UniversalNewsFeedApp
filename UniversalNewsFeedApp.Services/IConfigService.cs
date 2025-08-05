using UniversalNewsFeedApp.Model;

namespace UniversalNewsFeedApp.Services
{
    public interface IConfigService
    {
        IAsyncEnumerable<SourceConfig> convertJsonToObj();
    }
}
