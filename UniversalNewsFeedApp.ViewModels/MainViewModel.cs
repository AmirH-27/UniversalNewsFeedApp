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
            RefreshCommand = new RelayCommand(RefreshNews);
            TextMessage = "News Feed Application";

            RefreshNews();
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


        private void RefreshNews()
        {
            Articles.Clear();
            try
            {
                var allArticles = _configs
                    .SelectMany(config =>
                    {
                        var service = new UniversalNewsService(config, HtmlLoader);
                        return service.FetchNews();
                    })
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
        }
    }
}
