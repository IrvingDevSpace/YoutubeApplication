using PropertyChanged;

namespace YoutubeApplication.Views
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel
    {
        public bool IsLoading { get; set; } = false;
    }
}
