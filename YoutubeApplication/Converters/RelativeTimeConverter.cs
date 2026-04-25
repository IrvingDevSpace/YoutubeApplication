using System.Globalization;
using System.Windows.Data;

namespace YoutubeApplication.Converters
{
    public class RelativeTimeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
                return string.Empty;

            if (!DateTime.TryParse(value.ToString(), out DateTime dateTime))
                return value.ToString() ?? string.Empty;

            var timeSpan = DateTime.UtcNow - dateTime;

            if (timeSpan.TotalSeconds < 60)
                return "剛剛";
            if (timeSpan.TotalMinutes < 60)
                return $"{(int)timeSpan.TotalMinutes} 分鐘前";
            if (timeSpan.TotalHours < 24)
                return $"{(int)timeSpan.TotalHours} 小時前";
            if (timeSpan.TotalDays < 7)
                return $"{(int)timeSpan.TotalDays} 天前";
            if (timeSpan.TotalDays < 30)
                return $"{(int)(timeSpan.TotalDays / 7)} 週前";
            if (timeSpan.TotalDays < 365)
                return $"{(int)(timeSpan.TotalDays / 30)} 個月前";

            return $"{(int)(timeSpan.TotalDays / 365)} 年前";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException("時間格式不支援回傳轉換");
        }
    }
}
