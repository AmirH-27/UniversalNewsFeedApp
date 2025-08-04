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
        public List<SourceConfig> convertJsonToObj()
        {
            try
            {
                if (!_fileSystem.Exists(_configFilePath))
                {
                    throw new FileNotFoundException("Config file not found", _configFilePath);
                }
                var json = _fileSystem.ReadAllText(_configFilePath);
                var configs = JsonSerializer.Deserialize<List<SourceConfig>>(json);
                if (configs == null)
                {
                    throw new NullReferenceException("Configuration file is empty");
                }
                return configs;
            }
            catch (FileNotFoundException)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw new JsonException("Failed to deserialize JSON data.", ex);
            }
            catch (IOException ex)
            {
                throw new IOException("Error reading the file.", ex);
            }
            catch (Exception ex)
            { 
                throw new InvalidOperationException("An error occurred.", ex);
            }
        }
    }
}
