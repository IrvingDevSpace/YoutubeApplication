using System.Windows;
using System.Windows.Controls;

namespace YoutubeApplication.Components.VideoCardComponent
{
    /// <summary>
    /// VideoCardView.xaml 的互動邏輯
    /// </summary>
    public partial class VideoCardView : UserControl
    {
        private readonly VideoCardViewModel _vm = new(new VideoCardPresenter());

        public VideoCardView()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public VideoCard VideoCard
        {
            get => (VideoCard)GetValue(VideoCardProperty);
            set => SetValue(VideoCardProperty, value);
        }
        public static readonly DependencyProperty VideoCardProperty =
            DependencyProperty.Register(nameof(VideoCard), typeof(VideoCard), typeof(VideoCardView),
                new PropertyMetadata(new VideoCard(), (d, e) => ((VideoCardView)d)._vm.VideoCard = (VideoCard)e.NewValue));
    }
}
