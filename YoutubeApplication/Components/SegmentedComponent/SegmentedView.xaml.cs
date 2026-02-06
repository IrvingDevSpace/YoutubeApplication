using System.Collections;
using System.Windows;
using System.Windows.Controls;

namespace YoutubeApplication.Components.SegmentedComponent
{
    /// <summary>
    /// SegmentedView.xaml 的互動邏輯
    /// </summary>
    public partial class SegmentedView : UserControl
    {
        public SegmentedView()
        {
            InitializeComponent();
        }

        public string Title
        {
            get => (string)GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }

        public static readonly DependencyProperty TitleProperty =
            DependencyProperty.Register(nameof(Title), typeof(string), typeof(SegmentedView),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public IEnumerable Options
        {
            get => (IEnumerable)GetValue(OptionsProperty);
            set => SetValue(OptionsProperty, value);
        }
        public static readonly DependencyProperty OptionsProperty =
            DependencyProperty.Register(nameof(Options), typeof(IEnumerable), typeof(SegmentedView),
                new PropertyMetadata(null));

        public object Option
        {
            get => GetValue(OptionProperty);
            set => SetValue(OptionProperty, value);
        }

        public static readonly DependencyProperty OptionProperty =
            DependencyProperty.Register(nameof(Option), typeof(object), typeof(SegmentedView),
                new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
    }
}
