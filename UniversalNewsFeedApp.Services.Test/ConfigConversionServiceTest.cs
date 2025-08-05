using Moq;
using System.Text.Json;
using UniversalNewsFeedApp.Model;

namespace UniversalNewsFeedApp.Services.Test
{
    public class ConfigConversionServiceTest
    {
        private const string filePath = "Config/souces.json";
        [Fact]
        public async Task ConvertJsonToObj_ReturnsList_WhenJsonIsValid()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.ReadAllTextAsync(It.IsAny<string>()))
                .ReturnsAsync("[{\"PageUrl\":\"http://example.com\"}]");
            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var result = new List<SourceConfig>();
            await foreach (var config in service.convertJsonToObj())
            { 
                result.Add(config);
            }

            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("http://example.com", result[0].PageUrl);
        }

        [Fact]
        public async Task ConvertJsonToObj_FileNotFound()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem
            .Setup(fs => fs.ReadAllTextAsync(It.IsAny<string>()))
            .Throws(new FileNotFoundException());

            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var ex = await Assert.ThrowsAsync<FileNotFoundException>(async () =>
            {
                await foreach (var _ in service.convertJsonToObj()) { }

            });
            Assert.Equal("Config file not found.", ex.Message);
        }
        [Fact]
        public async Task ConvertJsonToObj_JsonException()
        {
            var mockFileSystem = new Mock<IFileSystem>();
            mockFileSystem.Setup(fs => fs.Exists(It.IsAny<string>())).Returns(true);
            mockFileSystem.Setup(fs => fs.ReadAllTextAsync(filePath)).ReturnsAsync("Invalid JSON");

            var service = new ConfigConversionService(filePath, mockFileSystem.Object);

            var ex = await Assert.ThrowsAsync<JsonException>(async () =>
            {
                await foreach (var _ in service.convertJsonToObj()) { }
            });
        }
    }
}
