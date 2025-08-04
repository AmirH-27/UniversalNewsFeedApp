using Moq;
using System.Text.Json;

namespace UniversalNewsFeedApp.Services.Test
{
    public class ConfigConversionServiceTest
    {
        private const string filePath = "Config/souces.json";
        [Fact]
        public void ConvertJsonToObj_ReturnsList_WhenJsonIsValid()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.ReadAllText(It.IsAny<string>()))
                .Returns("[{\"PageUrl\":\"http://example.com\"}]");
            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var result = service.convertJsonToObj();

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("http://example.com", result[0].PageUrl);
        }

        [Fact]
        public void ConvertJsonToObj_FileNotFound()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(false);
            
            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var ex = Assert.Throws<FileNotFoundException>(() => service.convertJsonToObj());
            Assert.Equal("Config file not found", ex.Message);
        }
        [Fact]
        public void ConvertJsonToObj_JsonException()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.ReadAllText(filePath)).Returns("Invalid JSON");

            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var ex = Assert.Throws<JsonException>(() => service.convertJsonToObj());
            Assert.Equal("Failed to deserialize JSON data.", ex.Message);
        }
    }
}
