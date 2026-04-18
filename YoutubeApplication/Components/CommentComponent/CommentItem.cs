using PropertyChanged;
using YoutubeApplication.Enums;

namespace YoutubeApplication.Components.CommentComponent
{
    [AddINotifyPropertyChangedInterface]
    public class CommentItem
    {
        public string Id { get; set; }

        public string AuthorName { get; set; }

        public string ProfileImageUrl { get; set; }

        public string Text { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int LikeCount { get; set; }

        public RatingTag UserRating { get; set; }
    }
}
