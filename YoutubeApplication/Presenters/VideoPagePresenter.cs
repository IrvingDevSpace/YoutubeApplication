using YoutubeAPI;
using YoutubeApplication.Presenters.Base;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Presenters
{
    public class VideoPagePresenter : BasePresenter, IVideoPagePresenter
    {
        private readonly YoutubeContext _context;

        public VideoPagePresenter(YoutubeContext context)
        {
            _context = context;
        }
    }
}
