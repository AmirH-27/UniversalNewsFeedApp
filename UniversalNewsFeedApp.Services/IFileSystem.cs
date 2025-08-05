using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalNewsFeedApp.Services
{
    public interface IFileSystem
    {
        bool Exists(string path);
        Task<string> ReadAllTextAsync(string path);
    }
}
