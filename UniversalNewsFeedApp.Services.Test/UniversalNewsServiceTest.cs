using HtmlAgilityPack;
using Moq;
using UniversalNewsFeedApp.Model;

namespace UniversalNewsFeedApp.Services.Test
{
    public class UniversalNewsServiceTest
    {
        private static UniversalNewsService CreateService(SourceConfig config, string html)
        {
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var mockHtmlLoader = new Mock<IHtmlLoader>();
            mockHtmlLoader.Setup(x => x.LoadAsync(config.PageUrl)).ReturnsAsync(htmlDoc);

            return new UniversalNewsService(config, mockHtmlLoader.Object);
        }
        private SourceConfig GetDefaultConfig() => new SourceConfig
        {
            PageUrl = "https://test.com",
            BaseUrl = "https://test.com",
            LinkSelector = "//a[.//h2]",
            TitleSelector = ".//h2",
            UrlSelector = "href",
            Source = "test.com"
        };

        [Fact]
        public async Task FetchNewsReturnsEmptyList()
        {
            // Arrange
            var config = GetDefaultConfig();
            var html = "<html><body><div>No links here</div></body></html>";
            var service = CreateService(config, html);

            // Act
            var result = await service.FetchNews();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public async Task FetchNews_ReturnsArticles_WhenValidHtmlIsParsed()
        {
            // Arrange
            var config = GetDefaultConfig();
            var html = @"
            <html>
                <body>
                    <a class='news-link' href='/article1'>
                        <h2>Title One</h2>
                    </a>
                    <a class='news-link' href='/article2'>
                        <h2>Title Two</h2>
                    </a>
                </body>
            </html>";
            var service = CreateService(config, html);

            // Act
            var result = await service.FetchNews();

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Contains(result, a => a.Headline == "Title One" && a.Url == "https://test.com/article1");
            Assert.Contains(result, a => a.Headline == "Title Two" && a.Url == "https://test.com/article2");
        }

        [Fact]
        public async Task FetchNews_SkipsDuplicateTitles()
        {
            // Arrange
            var config = GetDefaultConfig();
            var html = @"
            <html>
                <body>
                    <a class='news-link' href='/article1'>
                        <h2>Title One</h2>
                    </a>
                    <a class='news-link' href='/article2'>
                        <h2>Title One</h2>
                    </a>
                </body>
            </html>";

            var service = CreateService(config, html);

            // Act
            var result = await service.FetchNews();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FetchNews_Extract_Url_IsNull_Or_WhiteSpace()
        {
            var config = GetDefaultConfig();
            config.UrlSelector = "";
            var html = @"
            <html>
                <body>
                    <a class='news-link'href='/article1'>
                        <h2>Should Not Appear</h2>
                    </a>
                </body>
            </html>";

            var service = CreateService(config, html);
            var result = await service.FetchNews();

            Assert.Empty(result);
        }
        [Theory]
        [InlineData("href", @"
                    <html>
                        <body>
                            <a class='news-link' href='/article1'>
                                <h2>Attribute Title</h2>
                            </a>
                        </body>
                    </html>")]
        [InlineData(".//span", @"
                    <html>
                        <body>
                            <a class='news-link'>
                                <span href='/node-url'></span>
                                <h2>Node Title</h2>
                            </a>
                        </body>
                    </html>")]
        public async Task FetchNews_Extract_Url_Correctly(string urlSelector,  string html)
        {
            // Arrange
            var config = GetDefaultConfig();
            config.UrlSelector = urlSelector;

            var service = CreateService(config, html);
            
            // Act
            var result = await service.FetchNews();

            // Assert
            Assert.Single(result);
        }

        [Fact]
        public async Task FetchNews_SkipsNodeWhenEmptyHref()
        {
            var config = GetDefaultConfig();
            var html = @"
            <html>
                <body>
                    <a class='news-link'href=''>
                        <h2>Should Not Appear</h2>
                    </a>
                </body>
            </html>";

            var service = CreateService(config, html);
            var result = await service.FetchNews();

            Assert.Empty(result);
        }
        [Fact]
        public async Task FetchNews_SkipsNodeWhenEmptyTitle()
        {
            var config = GetDefaultConfig();
            var html = @"
            <html>
                <body>
                    <a class='news-link' href='/article1'>
                        
                    </a>
                    <a class='news-link' href='/article2'>
                        <h2>Title Two</h2>
                    </a>
                </body>
            </html>";

            var service = CreateService(config, html);
            var result = await service.FetchNews();

            Assert.Single(result);
        }

        [Fact]
        public async Task FetchNews_DecodesAndTrimsTitle()
        {
            var config = GetDefaultConfig();
            var html = @"
                <html>
                    <body>
                        <a class='news-link' href='/article1'>
                            <h2>           Hello &amp; World            </h2>
                        </a>
                    </body>
                /html>";

            var service = CreateService(config, html);
            var result = await service.FetchNews();

            Assert.Single(result);
            Assert.Equal("Hello & World", result[0].Headline);
        }
    }
}
