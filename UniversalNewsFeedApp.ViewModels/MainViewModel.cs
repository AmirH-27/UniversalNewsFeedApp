using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;
using UniversalNewsFeedApp.Model;
using UniversalNewsFeedApp.Services;

namespace UniversalNewsFeedApp.ViewModel
{
    public partial class MainViewModel : ObservableObject
    {
        private List<SourceConfig> _configs;
        public ObservableCollection<NewsArticle> Articles { get; set; } = new();
        public ICommand ClearTextMessageCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand OpenUrlCommand { get; }
        [ObservableProperty]
        private string textMessage;

        [ObservableProperty]
        private bool isLoading;

        public IUrlOpenerService UrlOpenerService { get; }
        public IConfigService ConfigService { get; }
        public IHtmlLoader HtmlLoader { get; }

        public MainViewModel(IUrlOpenerService urlOpenerService, IConfigService configService, IHtmlLoader htmlLoader)
        {
            UrlOpenerService = urlOpenerService;
            ConfigService = configService;
            HtmlLoader = htmlLoader;

            _configs = LoadConfigs();

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

        private List<SourceConfig> LoadConfigs()
        {
            return ConfigService.convertJsonToObj();
        }

        [RelayCommand]
        private async Task RefreshNewsAsync()
        {
            IsLoading = true;
            Articles.Clear();
            try
            {
                var fetchTasks = _configs.Select(config =>
                {
                    var service = new UniversalNewsService(config, HtmlLoader);
                    return service.FetchNews();
                });
                var results = await Task.WhenAll(fetchTasks);
                var allArticles = results
                        .SelectMany(list => list)
                        .DistinctBy(article => $"{article.Headline}|{article.Url}");

                foreach (var article in allArticles)
                {
                    Articles.Add(article);
                }
            }
            catch (ArgumentNullException)
            {
                TextMessage = "Error Loading Articles";
            }
            finally 
            {
                IsLoading = false; 
            }
        }
    }
}
