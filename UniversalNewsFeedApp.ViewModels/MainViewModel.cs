using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using UniversalNewsFeedApp.Model;
using UniversalNewsFeedApp.Services;

namespace UniversalNewsFeedApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        public ObservableCollection<NewsArticle> Articles { get; set; } = new();
        public ICommand RefreshCommand { get; }
        public ICommand OpenUrlCommand { get; }
        [ObservableProperty]
        private string textMessage;
        [ObservableProperty]
        private bool isLoading;
        [ObservableProperty]
        private int articlesLoadedCount;

        public IUrlOpenerService UrlOpenerService { get; }
        public IConfigService ConfigService { get; }
        public IHtmlLoader HtmlLoader { get; }

        public MainViewModel(IUrlOpenerService urlOpenerService, IConfigService configService, IHtmlLoader htmlLoader)
        {
            UrlOpenerService = urlOpenerService;
            ConfigService = configService;
            HtmlLoader = htmlLoader;

            OpenUrlCommand = new RelayCommand<NewsArticle>(OpenUrl);
            RefreshCommand = new AsyncRelayCommand(RefreshNewsAsync);
            TextMessage = "News Feed Application";

            _ = RefreshNewsAsync();
        }

        private void OpenUrl(NewsArticle? article)
        {
            if (article == null)
            {
                return;
            }

            UrlOpenerService.OpenUrl(article.Url);
        }

        private async Task RefreshNewsAsync()
        {
            IsLoading = true;
            Articles.Clear();
            ArticlesLoadedCount = 0;

            try
            {
                var task = new List<Task>();
                await foreach (var config in ConfigService.convertJsonToObj())
                {
                    task.Add(FetchNewsArticleAsync(config));
                }
                await Task.WhenAll(task);
            }
            catch (ArgumentNullException)
            {
                TextMessage = "Error loading articles.";
            }
            finally
            {
                IsLoading = false;
                
            }
        }

        private async Task FetchNewsArticleAsync(SourceConfig config)
        {
            var service = new UniversalNewsService(config, HtmlLoader);

            await foreach (var article in service.FetchNewsAsync())
            {
                await Task.Delay(100);
                Articles.Add(article);
                ArticlesLoadedCount++;
            }
        }
    }
}
