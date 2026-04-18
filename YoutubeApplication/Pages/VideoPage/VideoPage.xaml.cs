using System.Windows.Controls;
using YoutubeApplication.Context;
using YoutubeApplication.Presenters;

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
            DataContext = new VideoPageViewModel(new VideoPagePresenter(YoutubeContextProvider.Context));
        }
    }
}
