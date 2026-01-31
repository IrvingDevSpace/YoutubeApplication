using PropertyChanged;

namespace YoutubeApplication
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel
    {
        public string WindowTitle { get; set; } = "YouTube";

        // 當前頁面的 ViewModel (可以是 LoginViewModel 或 HomeViewModel)
        // 這裡用 object 是因為 WPF 的 ContentControl 可以接受任何物件
        public object CurrentPage { get; set; }
    }
}
