using System.Windows.Controls;

namespace YoutubeApplication.Navigation
{
    public interface INavService
    {
        Frame Frame { get; set; }

        void Navigate(string key, params object[] args);
    }
}
