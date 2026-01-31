using System.ComponentModel;

namespace YoutubeApplication.Enums
{
    public enum Category
    {
        [Description("影片")]
        Video,

        [Description("頻道")]
        Channel,

        [Description("播放清單")]
        Playlist,

        [Description("電影")]
        Movie,

        [Description("Shorts")]
        Shorts
    }
}
