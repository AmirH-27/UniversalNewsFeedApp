using UniversalNewsFeedApp.Model;
using UniversalNewsFeedApp.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using System.Windows.Input;
using System.Linq;
using System.Collections.Generic;

namespace UniversalNewsFeedApp.ViewModel
{
    // ViewModel for managing and displaying news articles
    public class MainViewModel
    {
        // Collection of news articles to display in the UI
        public ObservableCollection<NewsArticle> Articles { get; set; } = new();
        public ICommand RefreshCommand { get; set; }

        public MainViewModel()
        {
            RefreshCommand = new RelayCommand(RefreshNews);
            RefreshNews(null);// Load initial articles from the configured sources

        }

        private void RefreshNews(object obj)
        {
            Articles.Clear(); // Clear existing articles before fetching new ones
            try
            {
                // Read the source configuration from the JSON file

                var json = File.ReadAllText("Config/sources.json");
                List<SourceConfig> configs = JsonSerializer.Deserialize<List<SourceConfig>>(json);

                foreach (var config in configs)
                {
                    // For each source config, fetch news articles
                    var service = new UniversalNewsService(config);
                    var fetched = service.FetchNews();

                    foreach (var article in fetched)
                    {
                        // Check for duplicates by Headline + URL
                        bool isDuplicate = Articles.Any(a => a.Headline == article.Headline && a.Url == article.Url);
                        if (!isDuplicate)
                        {
                            Articles.Add(article);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading news: {ex.Message}");
            }
        }
    }
}
