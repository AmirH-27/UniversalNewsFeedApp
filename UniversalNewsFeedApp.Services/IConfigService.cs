using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniversalNewsFeedApp.Model;

namespace UniversalNewsFeedApp.Services
{
    public interface IConfigService
    {
        public List<SourceConfig> convertJsonToObj();
    }
}
