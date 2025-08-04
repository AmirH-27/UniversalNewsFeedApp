using System.Diagnostics;
using System.Windows.Controls;
using UniversalNewsFeedApp.Services;
using UniversalNewsFeedApp.ViewModel;

namespace UniversalNewsFeedApp.Views
{
    public partial class MainViewer : UserControl
    {
        public MainViewer()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel(new UrlOpenerService(new ProcessWrapperService()), new ConfigConversionService("Config/sources.json", new FileService()), new HtmlLoader());
        }
    }
}
