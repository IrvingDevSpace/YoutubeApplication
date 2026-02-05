using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;
using YoutubeApplication.Components.PaginationComponent;
using YoutubeApplication.Components.VideoCardComponent;
using YoutubeApplication.Enums;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Views.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IHomePresenter _presenter;

        private List<SearchItem> _allSearchItems = []; // 暫存所有原始資料

        //public SearchBarViewModel SearchBar { get; } = new SearchBarViewModel();

        public PaginationViewModel Pagination { get; } = new PaginationViewModel();

        public Category SearchCategory { get; set; } = Category.Video;

        public ObservableCollection<VideoCardViewModel> VideoCards { get; set; } = [];

        public ICommand OnSearchCommand { get; }

        //public Func<string, Task> HandleSearchAsync { get; private set; }

        public HomeViewModel(IHomePresenter presenter)
        {
            _presenter = presenter;
            OnSearchCommand = new RelayCommand<string>(async (keyword) => await ExecuteSearchAsync(keyword));
            //HandleSearchAsync = ExecuteSearchAsync;
            Pagination.OnPageIndexChange += (index) => UpdateDisplayCards();
            Pagination.OnPageSizeChange += (size) => UpdateDisplayCards();
        }

        private async Task ExecuteSearchAsync(string keyword)
        {
            if (string.IsNullOrWhiteSpace(keyword)) return;

            if (IsLoading) return;

            IsLoading = true;

            try
            {
                var result = await _presenter.SearchByCategoryAsync(keyword, SearchCategory.ToString());

                if (!result.IsSuccess)
                {
                    Debug.WriteLine(result.Message);
                    return;
                }

                if (result.Data != null)
                {
                    _allSearchItems = result.Data;
                    Pagination.Total = _allSearchItems.Count;
                    UpdateDisplayCards();
                }
            }
            finally
            {
                IsLoading = false;
            }
        }

        private void UpdateDisplayCards()
        {
            VideoCards.Clear();

            int skip = (Pagination.PageIndex - 1) * Pagination.PageSize;
            var pagedData = _allSearchItems
                .Skip(skip)
                .Take(Pagination.PageSize);

            foreach (var item in pagedData)
            {
                var snippet = item.Snippet;

                var videoCardVm = new VideoCardViewModel(new VideoCardPresenter())
                {
                    VideoUrl = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                    Title = snippet.Title,
                    ImgSrc = snippet.Thumbnails.High.Url,
                    ChannelName = snippet.ChannelTitle,
                    Views = new Random().Next(1000, 1000000),
                    PublishedAt = snippet.PublishedAt
                };

                VideoCards.Add(videoCardVm);
            }
        }
    }
}
