using YoutubeAPI.Models.Comment;
using YoutubeAPI.Models.CommentThread;
using YoutubeAPI.Models.Subscription;
using YoutubeAPI.Models.Video;
using YoutubeApplication.Common;
using YoutubeApplication.Components.CommentComponent;
using YoutubeApplication.Enums;

namespace YoutubeApplication.Presenters.Interfaces
{
    public interface IVideoPagePresenter
    {
        Task<Result<ChannelListResponse>> GetChannelByIdAsync(string channelId);

        Task<Result<string>> GetSubscriptionIdAsync(string channelId);

        Task<Result<SubscriptionResponse>> SubscribeAsync(string channelId);

        Task<Result> UnsubscribeAsync(string subscriptionId);

        Task<Result<VideoListResponse>> GetStatisticsAsync(string videoId);

        Task<Result<RatingTag>> GetRatingAsync(string videoId);

        Task<Result> RateAsync(string videoId, RatingTag rating);

        Task<Result<CommentThreadListResponse>> GetCommentThreadListAsync(string videoId);

        Task<Result<List<CommentThreadItem>>> GetProcessedCommentThreadsAsync(string videoId);

        Task<Result<CommentListResponse>> GetCommentByIdAsync(string commentId);

        Task<Result> AddCommentThreadAsync(string videoId, string content);

        Task<Result> AddCommentAsync(string videoId, string content);
    }
}
