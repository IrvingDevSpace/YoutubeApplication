using System.Collections.ObjectModel;
using System.Windows.Input;
using YoutubeApplication.Common;
using YoutubeApplication.Components.CommentComponent;
using YoutubeApplication.Components.VideoCardComponent;
using YoutubeApplication.Enums;
using YoutubeApplication.Presenters.Interfaces;
using YoutubeApplication.Views;

namespace YoutubeApplication.Pages.VideoPage
{
    public class VideoPageViewModel : BaseViewModel
    {
        private readonly IVideoPagePresenter _presenter;

        public VideoCard VideoCard { get; set; }

        public string ChannelImageUrl { get; set; }

        public string ChannelName { get; set; }

        public string ChannelSubCount { get; set; }

        public ICommand SubscribeCommand { get; set; }

        public bool IsSubscribed { get; set; } = false;

        private string _currentSubscriptionId = "";

        public string LikeCount { get; set; } = "";

        public RatingTag UserRating { get; set; }

        public ICommand LikeCommand { get; set; }

        public ICommand DislikeCommand { get; set; }

        public string ViewCount { get; set; } = "0";

        public string Description { get; set; } = "";

        public string PublishDate { get; set; } = "";

        public string MyChannelImageUrl { get; set; } = App.MyChannel.Snippet.Thumbnails.High.Url;

        public bool IsCommentExpanded { get; set; } = false;

        public ICommand CommentExpandCmd { get; set; }

        public ICommand CommentCancelCmd { get; set; }

        public ICommand SubmitCommentCmd { get; set; }

        public ICommand SubmitSubCommentCmd { get; set; }

        public ICommand UpdateCommentCmd { get; set; }

        public ICommand DelCommentCmd { get; set; }

        public ObservableCollection<CommentThreadItem> CommentThreadItems { get; set; } = [];

        public VideoPageViewModel(IVideoPagePresenter presenter)
        {
            _presenter = presenter;

            SubscribeCommand = new AsyncRelayCommand(ExecuteSubscribe);
            LikeCommand = new AsyncRelayCommand(() => ExecuteRating(RatingTag.Like));
            DislikeCommand = new AsyncRelayCommand(() => ExecuteRating(RatingTag.Dislike));
            CommentExpandCmd = new RelayCommand(() => IsCommentExpanded = true);
            CommentCancelCmd = new RelayCommand(() => IsCommentExpanded = false);
            SubmitCommentCmd = new AsyncRelayCommand<string>(SubmitCommentAsync);
            SubmitSubCommentCmd = new AsyncRelayCommand<(CommentThreadItem CommentThread, string ReplyText)>(SubmitSubCommentAsync);
            UpdateCommentCmd = new AsyncRelayCommand<(string CommentId, string Text)>(UpdateCommentAsync);
            DelCommentCmd = new AsyncRelayCommand<string>(DelCommentAsync);
        }

        public async Task ExecuteSubscribe()
        {
            if (!IsSubscribed)
                await _presenter.SubscribeAsync(VideoCard.ChannelId);
            else
                await _presenter.UnsubscribeAsync(_currentSubscriptionId);

            GetIsSubscribedTask();
        }

        public async Task ExecuteRating(RatingTag target)
        {
            var nextRating = (UserRating == target) ? RatingTag.None : target;

            await _presenter.RateAsync(VideoCard.VideoId, nextRating);

            LoadVideoDetailsTask();
        }

        public override async void ApplyDataParamsAsync(object[] data)
        {
            VideoCard = (VideoCard)data[0];

            await Task.WhenAll(
                GetCommentsTask(),
                GetChannelInfoTask(),
                GetIsSubscribedTask(),
                LoadVideoDetailsTask()
            );
        }

        private async Task GetChannelInfoTask()
        {
            var result = await _presenter.GetChannelByIdAsync(VideoCard.ChannelId);
            if (result.IsSuccess && result.Data?.items?.Count > 0)
            {
                var channel = result.Data.items[0];
                ChannelImageUrl = channel.snippet.Thumbnails.High.Url;
                ChannelName = channel.snippet.Title;
                ChannelSubCount = channel.statistics.subscriberCount;
            }
        }

        private async Task GetIsSubscribedTask()
        {
            var result = await _presenter.GetSubscriptionIdAsync(VideoCard.ChannelId);

            if (result.IsSuccess && result.Data != null)
            {
                IsSubscribed = true;
                _currentSubscriptionId = result.Data;
            }
            else
            {
                IsSubscribed = false;
                _currentSubscriptionId = string.Empty;
            }
        }

        private async Task LoadVideoDetailsTask()
        {
            var statsTask = _presenter.GetStatisticsAsync(VideoCard.VideoId);
            var ratingTask = _presenter.GetRatingAsync(VideoCard.VideoId);

            await Task.WhenAll(statsTask, ratingTask);

            var stats = await statsTask;
            var rating = await ratingTask;

            if (stats.IsSuccess && stats.Data?.Items?.Count > 0)
            {
                var videoInfo = stats.Data.Items[0];
                LikeCount = videoInfo.Statistics.LikeCount;
                ViewCount = videoInfo.Statistics.ViewCount;
                Description = videoInfo.Snippet?.Description ?? "";
                PublishDate = videoInfo.Snippet?.PublishedAt.ToString("yyyy年MM月dd日") ?? "";
            }

            if (rating.IsSuccess)
                UserRating = rating.Data;
        }

        private async Task GetCommentsTask()
        {
            CommentThreadItems.Clear();
            var result = await _presenter.GetProcessedCommentThreadsAsync(VideoCard.VideoId);
            if (result.IsSuccess && result.Data != null)
                CommentThreadItems = new ObservableCollection<CommentThreadItem>(result.Data);
        }

        public async Task SubmitCommentAsync(string replyText)
        {
            var result = await _presenter.AddCommentThreadAsync(VideoCard.VideoId, replyText);
            if (result.IsSuccess && result.Data != null)
                CommentThreadItems.Insert(0, new CommentThreadItem { TopLevelComment = result.Data });
        }

        public async Task SubmitSubCommentAsync((CommentThreadItem CommentThread, string ReplyText) parameter)
        {
            var (commentThread, replyText) = parameter;

            var result = await _presenter.AddCommentAsync(commentThread.TopLevelComment.Id, replyText);

            if (result.IsSuccess && result.Data != null)
                commentThread.Replies.Insert(0, result.Data);
        }

        public async Task UpdateCommentAsync((string commentId, string text) parameter)
        {
            var (commentId, text) = parameter;
            await _presenter.UpdateCommentAsync(commentId, text);
            await GetCommentsTask();
        }

        public async Task DelCommentAsync(string commentId)
        {
            await _presenter.DelCommentAsync(commentId);
            await GetCommentsTask();
        }
    }
}
