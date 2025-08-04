using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniversalNewsFeedApp.Services
{
    public interface IFileSystem
    {
        public bool Exists(string path);
        public string ReadAllText(string path);
    }
}
