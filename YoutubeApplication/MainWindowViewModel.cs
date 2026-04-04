using PropertyChanged;

namespace YoutubeApplication
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public string WindowTitle { get; set; } = "YouTube";

        public object CurrentVm { get; set; }
    }
}
