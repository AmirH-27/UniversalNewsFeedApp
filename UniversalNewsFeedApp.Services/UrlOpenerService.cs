using System.Diagnostics;

namespace UniversalNewsFeedApp.Services
{
    public class UrlOpenerService : IUrlOpenerService
    {
        public IProcessWrapperService ProcessWrapperService { get; }

        public UrlOpenerService(IProcessWrapperService processWrapperService)
        {
            ProcessWrapperService = processWrapperService;
        }

        public void OpenUrl(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                return;
            }

            try
            {
                ProcessWrapperService.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true,
                });
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Unable to open URL: {url}", ex);
            }
        }
    }
}
