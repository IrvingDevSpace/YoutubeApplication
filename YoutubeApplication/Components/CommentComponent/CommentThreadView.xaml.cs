using System.Windows;
using System.Windows.Controls;

namespace YoutubeApplication.Components.CommentComponent
{
    /// <summary>
    /// CommentThreadView.xaml 的互動邏輯
    /// </summary>
    public partial class CommentThreadView : UserControl
    {
        public CommentThreadView()
        {
            InitializeComponent();
        }

        #region 外部 API (Dependency Properties)

        public CommentThreadItem CommentThread
        {
            get => (CommentThreadItem)GetValue(CommentThreadProperty);
            set => SetValue(CommentThreadProperty, value);
        }

        public static readonly DependencyProperty CommentThreadProperty =
            DependencyProperty.Register(
                nameof(CommentThread),
                typeof(CommentThreadItem),
                typeof(CommentThreadView),
                new PropertyMetadata()
            );

        ///// <summary>
        ///// 點擊
        ///// </summary>
        //public ICommand OnClickCommand
        //{
        //    get { return (ICommand)GetValue(OnClickCommandProperty); }
        //    set { SetValue(OnClickCommandProperty, value); }
        //}

        //public static readonly DependencyProperty OnClickCommandProperty =
        //    DependencyProperty.Register(
        //        nameof(OnClickCommand),
        //        typeof(ICommand),
        //        typeof(VideoCardView),
        //        new PropertyMetadata()
        //    );

        #endregion

        //public ICommand ClickCommand { get; }

        //private async Task ExecuteClickAsync(VideoCard videoCard)
        //{
        //    await OnClickCommand.ExecuteAsync(videoCard);
        //}
    }
}
