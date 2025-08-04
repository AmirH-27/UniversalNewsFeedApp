using System.Diagnostics;

namespace UniversalNewsFeedApp.Services;

public class ProcessWrapperService : IProcessWrapperService
{
    public Process? Start(ProcessStartInfo processStartInfo)
    {
        return Process.Start(processStartInfo);
    }
}
