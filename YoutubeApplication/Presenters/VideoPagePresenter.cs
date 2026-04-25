using YoutubeAPI;
using YoutubeAPI.Models.Comment;
using YoutubeAPI.Models.CommentThread;
using YoutubeAPI.Models.Subscription;
using YoutubeAPI.Models.Video;
using YoutubeApplication.Common;
using YoutubeApplication.Enums;
using YoutubeApplication.Presenters.Base;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Presenters
{
    public class VideoPagePresenter : BasePresenter, IVideoPagePresenter
    {
        private readonly YoutubeContext _context;

        public VideoPagePresenter(YoutubeContext context)
        {
            _context = context;
        }

        public async Task<Result<ChannelListResponse>> GetChannelByIdAsync(string channelId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Channel.GetChannelByIdAsync(channelId);
            });
        }

        public async Task<Result<string>> GetSubscriptionIdAsync(string channelId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Subscription.GetSubscriptionIdAsync(channelId);
            });
        }

        public async Task<Result<SubscriptionResponse>> SubscribeAsync(string channelId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Subscription.SubscribeToChannelAsync(channelId);
            });
        }

        public async Task<Result> UnsubscribeAsync(string subscriptionId)
        {
            return await ExecuteAsync(async () =>
            {
                await _context.Subscription.UnsubscribeAsync(subscriptionId);
            });
        }

        public async Task<Result<VideoListResponse>> GetStatisticsAsync(string videoId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Video.GetStatisticsAsync(videoId);
            });
        }

        public async Task<Result<RatingTag>> GetRatingAsync(string videoId)
        {
            return await ExecuteAsync(async () =>
            {
                var result = await _context.Video.GetRatingAsync(videoId);
                var rating = result?.Items?.FirstOrDefault()?.Rating;

                return rating.ToRatingTag();
            });
        }

        public async Task<Result> RateAsync(string videoId, RatingTag rating)
        {
            return await ExecuteAsync(async () =>
            {
                await _context.Video.RateAsync(videoId, rating.ToApiString());
            });
        }

        public async Task<Result<CommentThreadListResponse>> GetCommentThreadListAsync(string videoId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.CommentThread.GetCommentsAsync(videoId);
            });
        }

        public async Task<Result<CommentListResponse>> GetCommentByIdAsync(string commentId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Comment.GetByIdAsync(commentId);
            });
        }
    }
}
