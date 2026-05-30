using PropertyChanged;

namespace YoutubeApplication.Components.PlaylistComponent
{
    [AddINotifyPropertyChangedInterface]
    public class PlaylistItemVm
    {
        public string PlaylistId { get; set; }

        public string Title { get; set; }

        public string ImgUrl { get; set; }

        public bool IsSelected { get; set; }
    }
}
