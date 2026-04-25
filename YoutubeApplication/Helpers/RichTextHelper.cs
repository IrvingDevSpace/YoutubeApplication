using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using YoutubeApplication.Enums;
using YoutubeApplication.Models;

namespace YoutubeApplication.Helpers
{
    public static class RichTextHelper
    {
        public static readonly DependencyProperty SegmentsProperty =
            DependencyProperty.RegisterAttached(
                "Segments",
                typeof(IEnumerable<TextSegment>),
                typeof(RichTextHelper),
                new PropertyMetadata(null, OnSegmentsChanged));

        public static void SetSegments(DependencyObject element, IEnumerable<TextSegment> value)
            => element.SetValue(SegmentsProperty, value);

        public static IEnumerable<TextSegment> GetSegments(DependencyObject element)
            => (IEnumerable<TextSegment>)element.GetValue(SegmentsProperty);

        private static void OnSegmentsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBlock textBlock)
            {
                textBlock.Inlines.Clear();
                var segments = e.NewValue as IEnumerable<TextSegment>;

                if (segments == null) return;

                var color = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#065FD4")); // YouTube 藍

                foreach (var segment in segments)
                {
                    switch (segment.Type)
                    {
                        case SegmentType.Text:
                            textBlock.Inlines.Add(new Run(segment.Content));
                            break;

                        case SegmentType.Url:
                            var hyperlink = new Hyperlink(new Run(segment.Content))
                            {
                                Foreground = color,
                                TextDecorations = null,
                                NavigateUri = new Uri(segment.Content.StartsWith("http") ? segment.Content : $"https://{segment.Content}")
                            };

                            hyperlink.RequestNavigate += (s, e) =>
                            {
                                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri) { UseShellExecute = true });
                                e.Handled = true;
                            };

                            textBlock.Inlines.Add(hyperlink);
                            break;

                        case SegmentType.Timecode:
                            var timeHyperlink = new Hyperlink(new Run(segment.Content))
                            {
                                Foreground = color,
                                TextDecorations = null
                            };

                            timeHyperlink.Click += (s, args) => { };

                            textBlock.Inlines.Add(timeHyperlink);
                            break;
                    }
                }
            }
        }
    }
}
