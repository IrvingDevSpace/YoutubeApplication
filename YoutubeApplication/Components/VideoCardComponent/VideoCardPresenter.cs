using System.Diagnostics;

namespace YoutubeApplication.Components.VideoCardComponent
{
    public class VideoCardPresenter : IVideoCardPresenter
    {
        public void OpenVideo(string videoUrl)
        {
            Process.Start(new ProcessStartInfo(videoUrl) { UseShellExecute = true });
        }
    }
}
