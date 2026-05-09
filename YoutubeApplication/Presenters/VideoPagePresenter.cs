using System.Collections.ObjectModel;
using YoutubeAPI;
using YoutubeAPI.Models;
using YoutubeAPI.Models.Comment;
using YoutubeAPI.Models.CommentThread;
using YoutubeAPI.Models.Subscription;
using YoutubeAPI.Models.Video;
using YoutubeApplication.Common;
using YoutubeApplication.Components.CommentComponent;
using YoutubeApplication.Enums;
using YoutubeApplication.Helpers;
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

        public async Task<Result<List<CommentThreadItem>>> GetProcessedCommentThreadsAsync(string videoId)
        {
            return await ExecuteAsync(async () =>
            {
                var response = await _context.CommentThread.GetCommentsAsync(videoId);
                if (response?.Items == null) return [];

                var tasks = response.Items.Select(async c => new CommentThreadItem
                {
                    // 處理主留言
                    TopLevelComment = await MapToCommentItemInternal(c.Snippet.TopLevelComment.Snippet, c.Snippet.TopLevelComment.Id),

                    // 處理回覆 (Replies)
                    Replies = new ObservableCollection<CommentItem>(
                        await Task.WhenAll(c.Replies?.Comments?.Select(x => MapToCommentItemInternal(x.Snippet, x.Id)) ?? Array.Empty<Task<CommentItem>>())
                    )
                });

                var results = await Task.WhenAll(tasks);
                return results.ToList();
            });
        }

        private async Task<CommentItem> MapToCommentItemInternal(CommentSnippet s, string id)
        {
            async Task<RatingTag> FetchRatingAsync(string commentId)
            {
                var comment = await GetCommentByIdAsync(commentId);
                return comment.Data?.Items?.FirstOrDefault()?.Snippet?.ViewerRating.ToRatingTag() ?? RatingTag.None;
            }

            return new CommentItem
            {
                Id = id,
                AuthorChannelId = s.AuthorChannelId.Value,
                AuthorName = s.AuthorDisplayName,
                ProfileImageUrl = s.AuthorProfileImageUrl,
                Text = s.TextOriginal,
                TextSegments = CommentHelper.ParseComment(s.TextOriginal),
                UpdatedAt = s.UpdatedAt,
                LikeCount = s.LikeCount,
                UserRating = await FetchRatingAsync(id)
            };
        }

        public async Task<Result<CommentListResponse>> GetCommentByIdAsync(string commentId)
        {
            return await ExecuteAsync(async () =>
            {
                return await _context.Comment.GetByIdAsync(commentId);
            });
        }

        public async Task<Result<CommentItem>> AddCommentThreadAsync(string videoId, string content)
        {
            return await ExecuteAsync(async () =>
            {
                var request = new CommentThreadCreateRequest
                {
                    Snippet = new CommentThreadSnippet
                    {
                        VideoId = videoId,
                        TopLevelComment = new TopLevelComment
                        {
                            Snippet = new CommentSnippet
                            {
                                TextOriginal = content
                            }
                        }
                    }
                };
                var commentThread = await _context.CommentThread.AddCommentAsync(request);
                var comment = commentThread.Snippet.TopLevelComment;
                var commentItem = await MapToCommentItemInternal(comment.Snippet, comment.Id);
                return commentItem;
            });
        }

        public async Task<Result<CommentItem>> AddCommentAsync(string parentId, string content)
        {
            return await ExecuteAsync(async () =>
            {
                var request = new CommentAddRequest
                {
                    Snippet = new CommentSnippet
                    {
                        ParentId = parentId,
                        TextOriginal = content
                    }
                };
                var comment = await _context.Comment.CreateAsync(request);
                var commentItem = await MapToCommentItemInternal(comment.Snippet, comment.Id);

                return commentItem;
            });
        }

        public async Task<Result> UpdateCommentAsync(string commentId, string text)
        {
            var request = new CommentUpdateRequest
            {
                Id = commentId,
                Snippet = new CommentSnippet
                {
                    TextOriginal = text
                }
            };

            return await ExecuteAsync(async () =>
            {
                await _context.Comment.UpdateAsync(request);
            });
        }

        public async Task<Result> DelCommentAsync(string commentId)
        {
            return await ExecuteAsync(async () =>
            {
                await _context.Comment.DeleteAsync(commentId);
            });
        }
    }
}
