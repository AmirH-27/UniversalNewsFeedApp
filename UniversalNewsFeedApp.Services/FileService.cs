namespace UniversalNewsFeedApp.Services;

public class FileService : IFileSystem
{
    public bool Exists(string path)
    {
        return File.Exists(path);
    }

    public string ReadAllText(string path)
    {
        return File.ReadAllText(path);
    }
}
