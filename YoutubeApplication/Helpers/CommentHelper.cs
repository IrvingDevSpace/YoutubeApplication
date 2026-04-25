using System.Text.RegularExpressions;
using YoutubeApplication.Enums;
using YoutubeApplication.Models;

namespace YoutubeApplication.Helpers
{
    public static class CommentHelper
    {
        // 匹配時間軸 (例如 1:23, 01:23, 1:02:34)
        private static readonly Regex TimecodeRegex = new Regex(@"\b(?:[0-5]?\d:)?(?:[0-5]?\d):[0-5]\d\b", RegexOptions.Compiled);

        // 匹配 http/https 開頭的網址
        private static readonly Regex UrlRegex = new Regex(@"https?:\/\/[^\s]+", RegexOptions.Compiled);

        public static List<TextSegment> ParseComment(string rawText)
        {
            var segments = new List<TextSegment>();
            if (string.IsNullOrWhiteSpace(rawText)) return segments;

            // 將時間與網址合併成一個聯合的 Regex
            var pattern = $"({TimecodeRegex})|({UrlRegex})";
            var matches = Regex.Matches(rawText, pattern);

            int currentIndex = 0;

            foreach (Match match in matches)
            {
                if (match.Index > currentIndex)
                {
                    segments.Add(new TextSegment
                    {
                        Content = rawText.Substring(currentIndex, match.Index - currentIndex),
                        Type = SegmentType.Text
                    });
                }

                // 判斷是時間還是網址
                if (TimecodeRegex.IsMatch(match.Value))
                {
                    segments.Add(new TextSegment
                    {
                        Content = match.Value,
                        Type = SegmentType.Timecode,
                    });
                }
                else if (UrlRegex.IsMatch(match.Value))
                {
                    segments.Add(new TextSegment
                    {
                        Content = match.Value,
                        Type = SegmentType.Url,
                    });
                }

                currentIndex = match.Index + match.Length;
            }

            // 處理剩餘的純文字
            if (currentIndex < rawText.Length)
            {
                segments.Add(new TextSegment
                {
                    Content = rawText.Substring(currentIndex),
                    Type = SegmentType.Text
                });
            }

            return segments;
        }
    }
}
