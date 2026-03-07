using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.VideoCardComponent
{
    /// <summary>
    /// VideoCardView.xaml 的互動邏輯
    /// </summary>
    public partial class VideoCardView : UserControl
    {
        public VideoCardView()
        {
            InitializeComponent();

            ClickCommand = new AsyncRelayCommand<string>(
                ExecuteClickAsync,
                videoUrl => !string.IsNullOrEmpty(videoUrl)
            );
        }

        #region 外部 API (Dependency Properties)

        public VideoCard VideoCard
        {
            get => (VideoCard)GetValue(VideoCardProperty);
            set => SetValue(VideoCardProperty, value);
        }

        public static readonly DependencyProperty VideoCardProperty =
            DependencyProperty.Register(
                nameof(VideoCard),
                typeof(VideoCard),
                typeof(VideoCardView),
                new PropertyMetadata()
            );

        /// <summary>
        /// 點擊
        /// </summary>
        public ICommand OnClickCommand
        {
            get { return (ICommand)GetValue(OnClickCommandProperty); }
            set { SetValue(OnClickCommandProperty, value); }
        }

        public static readonly DependencyProperty OnClickCommandProperty =
            DependencyProperty.Register(
                nameof(OnClickCommand),
                typeof(ICommand),
                typeof(VideoCardView),
                new PropertyMetadata()
            );

        #endregion

        public ICommand ClickCommand { get; }

        private async Task ExecuteClickAsync(string url)
        {
            await OnClickCommand.ExecuteAsync(url);
        }
    }
}
