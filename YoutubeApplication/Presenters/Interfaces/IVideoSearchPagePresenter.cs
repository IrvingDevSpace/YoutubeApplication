using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;

namespace YoutubeApplication.Presenters.Interfaces
{
    internal interface IVideoSearchPagePresenter
    {
        Task<Result<List<SearchItem>>> SearchByCategoryAsync(string text, string category);

        Task<Result<List<SearchItem>>> SearchAsync(YouTubeSearchReq req);
    }
}
