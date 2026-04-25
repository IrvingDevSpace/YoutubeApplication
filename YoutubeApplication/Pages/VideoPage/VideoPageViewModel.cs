using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows.Input;
using YoutubeAPI.Models.Comment;
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

        public ObservableCollection<CommentThreadItem> CommentThreadItems { get; set; } = [];

        public VideoPageViewModel(IVideoPagePresenter presenter)
        {
            _presenter = presenter;

            SubscribeCommand = new AsyncRelayCommand(ExecuteSubscribe);
            LikeCommand = new AsyncRelayCommand(() => ExecuteRating(RatingTag.Like));
            DislikeCommand = new AsyncRelayCommand(() => ExecuteRating(RatingTag.Dislike));
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

            await _presenter.RateAsync(VideoCard.VideoId, UserRating);

            LoadVideoDetailsTask();
        }

        public override async void ApplyDataParamsAsync(object[] data)
        {
            VideoCard = (VideoCard)data[0];

            GetCommentsTask();
            GetChannelInfoTask();
            GetIsSubscribedTask();
            LoadVideoDetailsTask();
        }

        private async void GetChannelInfoTask()
        {
            var result = await _presenter.GetChannelByIdAsync(VideoCard.ChannelId);
            if (result.IsSuccess && result.Data?.items?.Count > 0)
            {
                var channel = result.Data.items[0];
                ChannelImageUrl = channel.snippet.thumbnails.high.url;
                ChannelName = channel.snippet.title;
                ChannelSubCount = channel.statistics.subscriberCount;
            }
        }

        private async void GetIsSubscribedTask()
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

        private async void LoadVideoDetailsTask()
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

        private async void GetCommentsTask()
        {
            CommentThreadItems.Clear();

            Debug.WriteLine("1");
            Debug.WriteLine(DateTime.Now);
            var result = await _presenter.GetCommentThreadListAsync(VideoCard.VideoId);
            Debug.WriteLine("2");
            Debug.WriteLine(DateTime.Now);
            if (!(result.IsSuccess && result.Data?.Items?.Count > 0))
                return;

            Debug.WriteLine("3");
            Debug.WriteLine(DateTime.Now);
            var commentThreads = result.Data.Items;

            var tasks = commentThreads.Select(async c =>
            {
                var item = new CommentThreadItem
                {
                    // 處理主留言
                    TopLevelComment = await MapToCommentItem(c.Snippet.TopLevelComment.Snippet, c.Snippet.TopLevelComment.Id),

                    // 處理回覆
                    Replies = new ObservableCollection<CommentItem>(
                        await Task.WhenAll(c.Replies?.Comments?.Select(x => MapToCommentItem(x.Snippet, x.Id)) ?? [])
                    )
                };
                return item;
            });

            var results = await Task.WhenAll(tasks);
            Debug.WriteLine("4");
            Debug.WriteLine(DateTime.Now);
            CommentThreadItems = new ObservableCollection<CommentThreadItem>(results);
            Debug.WriteLine("5");
            Debug.WriteLine(DateTime.Now);
        }

        private async Task<CommentItem> MapToCommentItem(CommentSnippet s, string id)
        {
            async Task<RatingTag> FetchRatingAsync(string id)
            {
                var comment = await _presenter.GetCommentByIdAsync(id);
                if (comment.Data?.Items?.Count > 0)
                    return comment.Data.Items[0].Snippet.ViewerRating.ToRatingTag();
                else
                    return RatingTag.None;
            }

            var item = new CommentItem
            {
                AuthorName = s.AuthorDisplayName,
                ProfileImageUrl = s.AuthorProfileImageUrl,
                Text = s.TextOriginal,
                UpdatedAt = s.UpdatedAt,
                LikeCount = s.LikeCount,
                UserRating = await FetchRatingAsync(id),
            };

            return item;
        }
    }
}
