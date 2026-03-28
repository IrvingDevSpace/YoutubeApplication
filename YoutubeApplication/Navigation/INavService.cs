namespace YoutubeApplication.Navigation
{
    public interface INavService
    {
        void Navigate(string key, params object[] args);
    }
}
