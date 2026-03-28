using System.Windows.Controls;

namespace YoutubeApplication.Pages.VideoPage
{
    /// <summary>
    /// VideoPage.xaml 的互動邏輯
    /// </summary>
    public partial class VideoPage : Page
    {
        public VideoPage()
        {
            InitializeComponent();
            DataContext = new VideoPageViewModel();
        }
    }
}
