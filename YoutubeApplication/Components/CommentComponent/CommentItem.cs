using PropertyChanged;
using YoutubeApplication.Enums;
using YoutubeApplication.Models;

namespace YoutubeApplication.Components.CommentComponent
{
    [AddINotifyPropertyChangedInterface]
    public class CommentItem
    {
        public string Id { get; set; }

        public string AuthorChannelId { get; set; }

        public bool IsMine => AuthorChannelId == App.MyChannel.ChannelId;

        public string AuthorName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Text { get; set; }

        public List<TextSegment> TextSegments { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int LikeCount { get; set; }

        public RatingTag UserRating { get; set; }
    }
}
