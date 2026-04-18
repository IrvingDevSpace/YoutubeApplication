using PropertyChanged;
using System.Collections.ObjectModel;

namespace YoutubeApplication.Components.CommentComponent
{
    [AddINotifyPropertyChangedInterface]
    public class CommentThreadItem
    {
        public CommentItem TopLevelComment { get; set; }

        public ObservableCollection<CommentItem> Replies { get; set; } = new();

        public int TotalReplyCount { get; set; }

        public bool HasReplies { get; set; }
    }
}
