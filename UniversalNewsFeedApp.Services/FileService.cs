
namespace UniversalNewsFeedApp.Services;

public class FileService : IFileSystem
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public Task<string> ReadAllTextAsync(string path)
    {
        return File.ReadAllTextAsync(path);
    }

}
