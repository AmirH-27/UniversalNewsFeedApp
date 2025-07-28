using UniversalNewsFeedApp.Model;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace UniversalNewsFeedApp.Views
{
    public partial class MainViewer : UserControl
    {
        public MainViewer()
        {
            InitializeComponent();
        }

        public void Headline_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (sender is TextBlock tb && tb.DataContext is NewsArticle article)
            {
                Console.WriteLine($"Opening URL: {article.Url}");
                Process.Start(new ProcessStartInfo(article.Url) { UseShellExecute = true });
            }
        }
    }
}
