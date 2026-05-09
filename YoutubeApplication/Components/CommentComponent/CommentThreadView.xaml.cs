using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeApplication.Components.CommentComponent
{
    /// <summary>
    /// CommentThreadView.xaml 的互動邏輯
    /// </summary>
    public partial class CommentThreadView : UserControl
    {
        private readonly CommentThreadVm _vm;

        public CommentThreadView()
        {
            InitializeComponent();
            _vm = new CommentThreadVm();
            Root.DataContext = _vm;
        }

        public CommentThreadItem CommentThread
        {
            get => (CommentThreadItem)GetValue(CommentThreadDp);
            set => SetValue(CommentThreadDp, value);
        }

        public static readonly DependencyProperty CommentThreadDp =
            DependencyProperty.Register(
                nameof(CommentThread),
                typeof(CommentThreadItem),
                typeof(CommentThreadView),
                new PropertyMetadata((d, e) => ((CommentThreadView)d)._vm.CommentThread = (CommentThreadItem)e.NewValue)
            );

        public ICommand OnSubCommentSubmitCmd
        {
            get { return (ICommand)GetValue(OnSubCommentSubmitCmdDp); }
            set { SetValue(OnSubCommentSubmitCmdDp, value); }
        }

        public static readonly DependencyProperty OnSubCommentSubmitCmdDp =
            DependencyProperty.Register(
                nameof(OnSubCommentSubmitCmd),
                typeof(ICommand),
                typeof(CommentThreadView),
                new PropertyMetadata((d, e) => ((CommentThreadView)d)._vm.OnSubCommentSubmitCmd = (ICommand)e.NewValue)
            );

        public ICommand OnSaveCommentCmd
        {
            get { return (ICommand)GetValue(OnSaveCommentCmdDp); }
            set { SetValue(OnSaveCommentCmdDp, value); }
        }

        public static readonly DependencyProperty OnSaveCommentCmdDp =
            DependencyProperty.Register(
                nameof(OnSaveCommentCmd),
                typeof(ICommand),
                typeof(CommentThreadView),
                new PropertyMetadata((d, e) => ((CommentThreadView)d)._vm.OnSaveCommentCmd = (ICommand)e.NewValue)
            );

        public ICommand OnDelCommentCmd
        {
            get { return (ICommand)GetValue(OnDelCommentCmdDp); }
            set { SetValue(OnDelCommentCmdDp, value); }
        }

        public static readonly DependencyProperty OnDelCommentCmdDp =
            DependencyProperty.Register(
                nameof(OnDelCommentCmd),
                typeof(ICommand),
                typeof(CommentThreadView),
                new PropertyMetadata((d, e) => ((CommentThreadView)d)._vm.OnDelCommentCmd = (ICommand)e.NewValue)
            );
    }
}
