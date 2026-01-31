using YoutubeAPI;
using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;
using YoutubeApplication.Presenters.Base;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Presenters
{
    public class HomePresenter : BasePresenter, IHomePresenter
    {
        private readonly YoutubeContext _context;

        public HomePresenter(YoutubeContext context)
        {
            _context = context;
        }

        public async Task<Result<List<SearchItem>>> SearchByCategoryAsync(string text, string category)
        {
            return await ExecuteAsync(async () =>
            {
                if (string.IsNullOrWhiteSpace(text))
                    throw new ArgumentException("搜尋關鍵字不能為空");

                var searchListResponse = await _context.Search.SearchByCategoryAsync(text, category);

                return searchListResponse?.Items?.ToList() ?? [];
            });
        }
    }
}
