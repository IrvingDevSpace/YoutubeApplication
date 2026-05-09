using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;
using YoutubeApplication.Components.AddCommentComponent;

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
            ReplyCommand = new AsyncRelayCommand<string>(ReplyAsync);
            ReplyExpandedCommand = new RelayCommand(() => IsReplyExpanded = true);
            ReplyCancelCommand = new RelayCommand(() => IsReplyExpanded = false);
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

        /// <summary>
        /// 送出事件
        /// </summary>
        public ICommand OnReplyCommand
        {
            get { return (ICommand)GetValue(OnReplyProperty); }
            set { SetValue(OnReplyProperty, value); }
        }

        public static readonly DependencyProperty OnReplyProperty =
            DependencyProperty.Register(
                nameof(OnReplyCommand),
                typeof(ICommand),
                typeof(AddCommentView),
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

        public static readonly DependencyProperty IsReplyExpandedProperty =
            DependencyProperty.Register(
                nameof(IsReplyExpanded),
                typeof(bool),
                typeof(CommentThreadView),
                new PropertyMetadata(false));

        public bool IsReplyExpanded
        {
            get => (bool)GetValue(IsReplyExpandedProperty);
            set => SetValue(IsReplyExpandedProperty, value);
        }

        public ICommand ReplyExpandedCommand { get; set; }

        public ICommand ReplyCancelCommand { get; set; }

        public ICommand ReplyCommand { get; set; }

        public async Task ReplyAsync(string comment)
        {
            await OnReplyCommand.ExecuteAsync(comment);
        }
    }
}
