using YoutubeAPI.Enums.Search;

namespace YoutubeApplication.Views.SearchFilter
{
    public class SearchFilterViewModel : BaseViewModel
    {
        /// <summary>
        /// 類型
        /// </summary>
        private readonly Dictionary<string, SearchType> _typeMap = new()
        {
            { "全部", SearchType.All },
            { "影片", SearchType.Video },
            { "Shorts", SearchType.Shorts },
            { "頻道", SearchType.Channel },
            { "播放清單", SearchType.Playlist },
            { "電影", SearchType.Movie },
        };
        public List<string> TypeOptions { get; }
        public string SelectedType { get; set; }

        /// <summary>
        /// 片長
        /// </summary>
        private readonly Dictionary<string, SearchDuration> _durationMap = new()
        {
            { "不限", SearchDuration.Any },
            { "3 分鐘內", SearchDuration.Short },
            { "3 到 20 分鐘", SearchDuration.Medium },
            { "超過 20 分鐘)", SearchDuration.Long },
        };
        public List<string> DurationOptions { get; }
        public string SelectedDuration { get; set; }

        /// <summary>
        /// 上傳日期
        /// </summary>
        private readonly Dictionary<string, SearchDate> _dateMap = new()
        {
            { "不限時間", SearchDate.Any },
            { "今天", SearchDate.Today },
            { "本週", SearchDate.Week },
            { "本月", SearchDate.Month },
            { "今年", SearchDate.Year },
        };
        public List<string> DateOptions { get; }
        public string SelectedDate { get; set; }

        /// <summary>
        /// 優先順序
        /// </summary>
        private readonly Dictionary<string, SearchOrder> _orderMap = new()
        {
            { "關聯性", SearchOrder.Relevance },
            { "熱門程度", SearchOrder.Popularity },
        };
        public List<string> OrderOptions { get; }
        public string SelectedOrder { get; set; }

        public SearchFilterViewModel()
        {
            TypeOptions = _typeMap.Keys.ToList();
            OrderOptions = _orderMap.Keys.ToList();
            DurationOptions = _durationMap.Keys.ToList();
            DateOptions = _dateMap.Keys.ToList();

            SelectedType = TypeOptions[0];
            SelectedOrder = OrderOptions[0];
            SelectedDuration = DurationOptions[0];
            SelectedDate = DateOptions[0];
        }
    }
}
