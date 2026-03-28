using YoutubeApplication.Views;

namespace YoutubeApplication.Pages.VideoPage
{
    public class VideoPageViewModel : BaseViewModel
    {
        public VideoPageViewModel()
        {
        }

        public string VideoId { get; set; }

        public override void ApplyDataParams(object[] data)
        {
            VideoId = (string)data[0];
        }
    }
}
