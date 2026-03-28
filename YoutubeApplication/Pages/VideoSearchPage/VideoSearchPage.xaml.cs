using System.Windows.Controls;

namespace YoutubeApplication.Pages.VideoSearchPage
{
    /// <summary>
    /// VideoSearchPage.xaml 的互動邏輯
    /// </summary>
    public partial class VideoSearchPage : Page
    {
        public VideoSearchPage()
        {
            InitializeComponent();
            DataContext = new VideoSearchPageViewModel();
        }
    }
}
