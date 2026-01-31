using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;

namespace YoutubeApplication.Presenters.Interfaces
{
    public interface IHomePresenter
    {
        Task<Result<List<SearchItem>>> SearchByCategoryAsync(string text, string category);
    }
}
