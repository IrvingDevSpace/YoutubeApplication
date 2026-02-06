using PropertyChanged;

namespace YoutubeApplication.Components.VideoCardComponent
{
    [AddINotifyPropertyChangedInterface]
    public class VideoCard
    {
        public string Title { get; set; }

        public string VideoUrl { get; set; }

        public string ImgSrc { get; set; }

        public string ChannelName { get; set; }

        public int Views { get; set; }

        public DateTime PublishedAt { get; set; }

        public string ViewSummary => $"觀看次數 : {Views}次 · {TimeAgo(PublishedAt)}";

        private string TimeAgo(DateTime publishedAt)
        {
            var diff = DateTime.Now - publishedAt;
            if (diff.TotalDays >= 365) return $"{(int)(diff.TotalDays / 365)} 年前";
            if (diff.TotalDays >= 30) return $"{(int)(diff.TotalDays / 30)} 個月前";
            if (diff.TotalDays >= 1) return $"{(int)diff.TotalDays} 天前";
            if (diff.TotalHours >= 1) return $"{(int)diff.TotalHours} 小時前";
            return $"{(int)diff.TotalMinutes} 分鐘前";
        }
    }
}
