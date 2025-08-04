using System.Diagnostics;

namespace UniversalNewsFeedApp.Services;

public interface IProcessWrapperService
{
    Process? Start(ProcessStartInfo processStartInfo);
}
