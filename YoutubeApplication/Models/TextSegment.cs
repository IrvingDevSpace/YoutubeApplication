using YoutubeApplication.Enums;

namespace YoutubeApplication.Models
{
    public class TextSegment
    {
        public string Content { get; set; } = string.Empty;
        public SegmentType Type { get; set; }
    }
}
