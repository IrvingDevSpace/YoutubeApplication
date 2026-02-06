using PropertyChanged;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.VideoCardComponent
{
    [AddINotifyPropertyChangedInterface]
    public class VideoCardViewModel
    {
        public VideoCard VideoCard { get; set; } = new();

        public ICommand OpenVideoCommand { get; }

        private readonly IVideoCardPresenter _presenter;

        public VideoCardViewModel(IVideoCardPresenter presenter)
        {
            _presenter = presenter;

            OpenVideoCommand = new RelayCommand(
                execute: () => _presenter.OpenVideo(VideoCard.VideoUrl),
                canExecute: () => !string.IsNullOrEmpty(VideoCard.VideoUrl)
            );
        }
    }
}
