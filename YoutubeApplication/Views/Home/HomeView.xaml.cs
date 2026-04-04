using System.Windows.Controls;

namespace YoutubeApplication.Views.Home
{
    /// <summary>
    /// HomeView.xaml 的互動邏輯
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            App.NavService.Frame = frame;
        }
    }
}
