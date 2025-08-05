using System.Text.Json;
using UniversalNewsFeedApp.Model;

namespace UniversalNewsFeedApp.Services
{
    public class ConfigConversionService : IConfigService
    {
        private readonly string _configFilePath;
        private readonly IFileSystem _fileSystem;
        public ConfigConversionService(string configFilePath, IFileSystem fileSystem)
        {
            _configFilePath = configFilePath;
            _fileSystem = fileSystem;
        }
        public async IAsyncEnumerable<SourceConfig> convertJsonToObj()
        {
            string json;
            try
            {
                json = await _fileSystem.ReadAllTextAsync(_configFilePath);
            }
            catch (FileNotFoundException)
            {
                yield break;
            }
            List<SourceConfig>? configs = null;

            try
            {
                configs = JsonSerializer.Deserialize<List<SourceConfig>>(json);
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize JSON data.", ex);
            }

            if (configs == null) yield break;

            foreach (var config in configs)
            {
                yield return config;
            }

        }
    }
}
