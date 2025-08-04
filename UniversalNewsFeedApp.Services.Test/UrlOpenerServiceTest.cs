using Moq;
using System;
using System.Diagnostics;

namespace UniversalNewsFeedApp.Services.Test
{
    public class UrlOpenerServiceTest
    {
        [Theory]
        [InlineData(null!)]
        [InlineData("")]
        public void OpenUrlWhenArticleIsNull(string? url)
        {
            //Arrange
            var mockProcessWrapperService = new Mock<IProcessWrapperService>();

            var urlOpener = new UrlOpenerService(mockProcessWrapperService.Object);
            
            //Act
            urlOpener.OpenUrl(url);

            //Assert
            mockProcessWrapperService.Verify(m => m.Start(It.IsAny<ProcessStartInfo>()), Times.Never());
        }

        [Fact]
        public void OpenUrlWhenUrlIsValid()
        {
            //Arrange
            var mockProcessWrapperService = new Mock<IProcessWrapperService>();
            mockProcessWrapperService.Setup(m => m.Start(It.IsAny<ProcessStartInfo>()));
            var urlOpener = new UrlOpenerService(mockProcessWrapperService.Object);

            //Act
            urlOpener.OpenUrl("https://www.google.com");

            //Assert
            mockProcessWrapperService.Verify(m => m.Start(It.IsAny<ProcessStartInfo>()), Times.Once());
        }

        [Fact]
        public void ProcessStartThrowsException()
        {
            //Arrange
            var mockProcessWrapperService = new Mock<IProcessWrapperService>();
            mockProcessWrapperService.Setup(m => m.Start(It.IsAny<ProcessStartInfo>())).Throws<Exception>();
            var urlOpener = new UrlOpenerService(mockProcessWrapperService.Object);

            //Act & Assert
            var exception = Assert.Throws<InvalidOperationException>(() => urlOpener.OpenUrl("https://www.google.com"));
            Assert.Equal("Unable to open URL: https://www.google.com", exception.Message);
        }
    }
}
