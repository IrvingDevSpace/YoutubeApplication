using YoutubeApplication.Common;

namespace YoutubeApplication.Presenters.Interfaces
{
    public interface ILoginPresenter
    {
        Task<Result> LoginAsync();
    }
}
