using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;
using YoutubeApplication.Components.VideoCardComponent;
using YoutubeApplication.Enums;
using YoutubeApplication.Presenters.Interfaces;
using YoutubeApplication.Views.SearchFilter;

namespace YoutubeApplication.Views.Home
{
    public class HomeViewModel : BaseViewModel
    {
        private readonly IHomePresenter _presenter;

        private List<SearchItem> _allSearchItems = []; // 暫存所有原始資料

        public string Keyword { get; set; }

        public int Total { get; set; }

        public int PageIndex { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public ICommand OnPageIndexChangeCommand { get; set; }

        public ICommand OnPageSizeChangeCommand { get; set; }

        public Category SearchCategory { get; set; } = Category.Video;

        public ObservableCollection<VideoCard> VideoCards { get; set; } = [];

        public ICommand OnSearchCommand { get; set; }

        public bool IsSearchFilterOpen { get; set; }

        public SearchFilterViewModel Filter { get; } = new SearchFilterViewModel();

        public ICommand OpenSearchFilterCommand { get; }
        public ICommand CloseSearchFilterCommand { get; }
        public ICommand ApplySearchFilterCommand { get; }

        public HomeViewModel(IHomePresenter presenter)
        {
            _presenter = presenter;
            OnSearchCommand = new AsyncRelayCommand(ExecuteSearchAsync);
            OnPageIndexChangeCommand = new RelayCommand(UpdateDisplayCards);
            OnPageSizeChangeCommand = new RelayCommand(UpdateDisplayCards);

            OpenSearchFilterCommand = new RelayCommand(() => IsSearchFilterOpen = true);
            CloseSearchFilterCommand = new RelayCommand(() => IsSearchFilterOpen = false);
            ApplySearchFilterCommand = new AsyncRelayCommand(ExecuteSearchAsync);
        }

        private async Task ExecuteSearchAsync()
        {
            if (string.IsNullOrWhiteSpace(Keyword)) return;
            var req = Filter.GetSearchRequest(Keyword);

            if (IsLoading) return;

            IsLoading = true;

            try
            {
                Debug.WriteLine($"Total, {Total} {PageIndex} {PageSize}");

                //await Task.Delay(10000);

                //var result = await _presenter.SearchByCategoryAsync(keyword, SearchCategory.ToString());

                var result = await _presenter.SearchAsync(req);

                if (!result.IsSuccess)
                {
                    Debug.WriteLine(result.Message);
                    return;
                }

                if (result.Data != null)
                {
                    _allSearchItems = result.Data;
                    Total = _allSearchItems.Count;
                    UpdateDisplayCards();
                }
            }
            finally
            {
                IsSearchFilterOpen = false;
                IsLoading = false;
            }
        }

        private void UpdateDisplayCards()
        {
            Debug.WriteLine($"UpdateDisplayCards, {Total} {PageIndex} {PageSize}");

            VideoCards.Clear();

            int skip = (PageIndex - 1) * PageSize;
            var currentPageItems = _allSearchItems
                .Skip(skip)
                .Take(PageSize);

            foreach (var item in currentPageItems)
            {
                var snippet = item.Snippet;

                var videoCardVm = new VideoCard
                {
                    Title = snippet.Title,
                    VideoUrl = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
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
