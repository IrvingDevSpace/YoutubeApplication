using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;
using YoutubeApplication.Components.VideoCardComponent;
using YoutubeApplication.Context;
using YoutubeApplication.Navigation;
using YoutubeApplication.Presenters;
using YoutubeApplication.Presenters.Interfaces;
using YoutubeApplication.Views;

namespace YoutubeApplication.Pages.VideoSearchPage
{
    public class VideoSearchPageViewModel : BaseViewModel
    {
        private readonly IVideoSearchPagePresenter _presenter;
        private readonly INavService _navService;

        public ObservableCollection<VideoCard> VideoCards { get; set; } = [];

        public ICommand OnVideoCardClickCommand { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int PageSize { get; set; } = 5;

        public int Total { get; set; }

        public ICommand OnPageChangeCommand { get; set; }

        public ICommand OnPageSizeChangeCommand { get; set; }

        private List<SearchItem> _allSearchItems = []; // 暫存所有原始資料


        public VideoSearchPageViewModel()
        {
            _presenter = new VideoSearchPagePresenter(YoutubeContextProvider.Context);

            OnVideoCardClickCommand = new RelayCommand<VideoCard>(
               videoCard => App.NavService.Navigate("VideoPage", videoCard)
            );

            OnPageChangeCommand = new RelayCommand(UpdateDisplayCards);
            OnPageSizeChangeCommand = new RelayCommand(UpdateDisplayCards);
        }

        private void UpdateDisplayCards()
        {
            Debug.WriteLine($"UpdateDisplayCards, {Total} {CurrentPage} {PageSize}");

            VideoCards.Clear();

            int skip = (CurrentPage - 1) * PageSize;
            var currentPageItems = _allSearchItems
                .Skip(skip)
                .Take(PageSize);

            foreach (var item in currentPageItems)
            {
                var snippet = item.Snippet;

                var videoCardVm = new VideoCard
                {
                    Title = snippet.Title,
                    //VideoUrl = $"https://www.youtube.com/watch?v={item.Id.VideoId}",
                    VideoId = item.Id.VideoId,
                    ImgSrc = snippet.Thumbnails.High.Url,
                    ChannelId = snippet.ChannelId,
                    ChannelName = snippet.ChannelTitle,
                    Views = new Random().Next(1000, 1000000),
                    PublishedAt = snippet.PublishedAt
                };

                VideoCards.Add(videoCardVm);
            }
        }

        public override async void ApplyDataParamsAsync(object[] data)
        {
            //Debug.WriteLine($"Total, {Total} {CurrentPage} {PageSize}");

            //await Task.Delay(10000);

            //var result = await _presenter.SearchByCategoryAsync(keyword, SearchCategory.ToString());
            var req = (YouTubeSearchReq)data[0];
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
    }
}
