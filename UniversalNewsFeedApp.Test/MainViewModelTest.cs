using Moq;
using UniversalNewsFeedApp.Services;
using UniversalNewsFeedApp.ViewModel;


namespace UniversalNewsFeedApp.Test
{
    public class MainViewModelTest
    {
        private MainViewModel _viewModel;
        private Mock<IUrlOpenerService> _mockUrlOpenerService;
        private Mock<IConfigService> _mockConfigService;
        private Mock<IHtmlLoader> _mockHtmlLoader;

        public MainViewModelTest()
        {
            _mockUrlOpenerService = new Mock<IUrlOpenerService>();
            _mockConfigService = new Mock<IConfigService>();
            _mockHtmlLoader = new Mock<IHtmlLoader>();
            _viewModel = new MainViewModel(_mockUrlOpenerService.Object, _mockConfigService.Object, _mockHtmlLoader.Object);
        }

        [Fact]
        public void OpenUrlWhenArticleIsNull()
        {
            //Arrange
            _mockUrlOpenerService.Setup(m => m.OpenUrl(It.IsAny<string>()));

            //Act
            _viewModel.OpenUrlCommand.Execute(null);

            //Assert
            _mockUrlOpenerService.Verify(m => m.OpenUrl(It.IsAny<string>()), Times.Never);
        }

    }
}
