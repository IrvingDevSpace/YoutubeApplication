using PropertyChanged;
using YoutubeApplication.Navigation;

namespace YoutubeApplication.Views
{
    [AddINotifyPropertyChangedInterface]
    public abstract class BaseViewModel : INavAware
    {
        public bool IsLoading { get; set; } = false;

        public abstract void ApplyDataParams(object[] data);
    }
}
