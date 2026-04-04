using System.Windows.Input;
using YoutubeAPI.Models.Search;
using YoutubeApplication.Common;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Views.Home
{
    public class HomeViewModel : BaseViewModel
    {

        public string Keyword { get; set; }

        public ICommand OnSearchCommand { get; set; }

        private YouTubeSearchReq _searchReq { get; set; } = new YouTubeSearchReq();

        public bool IsSearchFilterOpen { get; set; }

        public ICommand OpenSearchFilterCommand { get; }
        public ICommand OnSubmitCommand { get; }
        public ICommand CloseSearchFilterCommand { get; }

        public string CurrentVideoId { get; set; } = "yzpMHLHEm6I";

        public HomeViewModel(IHomePresenter presenter)
        {
            //_navService = navService;
            OnSearchCommand = new AsyncRelayCommand(
                ExecuteSearchAsync,
                () => !string.IsNullOrWhiteSpace(Keyword) && !IsLoading
            );

            OpenSearchFilterCommand = new RelayCommand(() => IsSearchFilterOpen = true);

            OnSubmitCommand = new RelayCommand<YouTubeSearchReq>(async (req) =>
            {
                _searchReq = req;
                if (!string.IsNullOrWhiteSpace(Keyword) && !IsLoading)
                    await ExecuteSearchAsync();

                IsSearchFilterOpen = false;
            });

            CloseSearchFilterCommand = new RelayCommand(() => IsSearchFilterOpen = false);
        }

        private async Task ExecuteSearchAsync()
        {
            try
            {
                IsLoading = true;
                _searchReq.Keyword = Keyword;
                App.NavService.Navigate("VideoSearchPage", _searchReq);

                ////Debug.WriteLine($"Total, {Total} {CurrentPage} {PageSize}");

                ////await Task.Delay(10000);

                ////var result = await _presenter.SearchByCategoryAsync(keyword, SearchCategory.ToString());

                //var result = await _presenter.SearchAsync(req);

                //if (!result.IsSuccess)
                //{
                //    Debug.WriteLine(result.Message);
                //    return;
                //}

                //if (result.Data != null)
                //{
                //    App.NavService.Navigate("VideoSearchPage", SearchPageVm, result.Data);
                //    //_allSearchItems = result.Data;
                //    //Total = _allSearchItems.Count;
                //    //UpdateDisplayCards();
                //}
            }
            finally
            {
                IsSearchFilterOpen = false;
                IsLoading = false;
            }
        }

        public override void ApplyDataParams(object[] data)
        {
            throw new NotImplementedException();
        }
    }
}
