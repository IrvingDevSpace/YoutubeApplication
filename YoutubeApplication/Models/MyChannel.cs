using static YoutubeAPI.Models.Subscription.ChannelListResponse;

namespace YoutubeApplication.Models
{
    public class MyChannel
    {
        public string ChannelId { get; set; }

        public Snippet Snippet { get; set; }
    }
}
