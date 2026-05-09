using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.AddCommentComponent
{
    /// <summary>
    /// AddCommentView.xaml 的互動邏輯
    /// </summary>
    public partial class AddCommentView : UserControl
    {
        public AddCommentView()
        {
            InitializeComponent();
            SubmitCommand = new AsyncRelayCommand(SubmitExecute, CanSubmitExecute);
            CancelCommand = new AsyncRelayCommand(CancelExecute);
        }

        #region 外部 API (Dependency Properties)

        /// <summary>
        /// 留言內容
        /// </summary>
        public string Content
        {
            get => (string)GetValue(ContentProperty);
            set => SetValue(ContentProperty, value);
        }

        public static readonly DependencyProperty ContentProperty =
            DependencyProperty.Register(
                nameof(Content),
                typeof(string),
                typeof(AddCommentView),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        /// <summary>
        /// 頭像連結
        /// </summary>
        public string ChannelImageUrl
        {
            get => (string)GetValue(ChannelImageUrlProperty);
            set => SetValue(ChannelImageUrlProperty, value);
        }

        public static readonly DependencyProperty ChannelImageUrlProperty =
            DependencyProperty.Register(
                nameof(ChannelImageUrl),
                typeof(string),
                typeof(AddCommentView),
                new FrameworkPropertyMetadata()
            );

        /// <summary>
        /// 送出事件
        /// </summary>
        public ICommand OnSubmitCommand
        {
            get { return (ICommand)GetValue(OnSubmitCommandProperty); }
            set { SetValue(OnSubmitCommandProperty, value); }
        }

        public static readonly DependencyProperty OnSubmitCommandProperty =
            DependencyProperty.Register(
                nameof(OnSubmitCommand),
                typeof(ICommand),
                typeof(AddCommentView),
                new PropertyMetadata()
            );

        /// <summary>
        // 取消事件
        /// </summary>
        public ICommand OnCancelCommand
        {
            get { return (ICommand)GetValue(OnCancelCommandProperty); }
            set { SetValue(OnCancelCommandProperty, value); }
        }

        public static readonly DependencyProperty OnCancelCommandProperty =
            DependencyProperty.Register(
                nameof(OnCancelCommand),
                typeof(ICommand),
                typeof(AddCommentView),
                new PropertyMetadata()
            );

        #endregion

        public ICommand SubmitCommand { get; }

        private bool CanSubmitExecute()
        {
            return !string.IsNullOrWhiteSpace(Content) &&
                   (OnSubmitCommand?.CanExecute(Content) ?? true);
        }

        private async Task SubmitExecute()
        {
            await OnSubmitCommand.ExecuteAsync(Content);
            Content = "";
        }

        public ICommand CancelCommand { get; }

        private async Task CancelExecute()
        {
            await OnCancelCommand.ExecuteAsync();
            Content = "";
        }
    }
}
