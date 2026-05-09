using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace YoutubeApplication.Components.AddCommentComponent
{
    /// <summary>
    /// AddCommentView.xaml 的互動邏輯
    /// </summary>
    public partial class AddCommentView : UserControl
    {
        private readonly AddCommentVm _vm;

        public AddCommentView()
        {
            InitializeComponent();
            _vm = new AddCommentVm();
            Root.DataContext = _vm;
            IsVisibleChanged += AddCommentView_IsVisibleChanged;
        }

        private void AddCommentView_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is false) return;

            Dispatcher.BeginInvoke(new Action(() =>
            {
                ReplyTextBox.Focus();
                Keyboard.Focus(ReplyTextBox);
            }), DispatcherPriority.Input);
        }

        /// <summary>
        /// 頻道頭像連結
        /// </summary>
        public string ChannelImageUrl
        {
            get => (string)GetValue(ChannelImageUrlDp);
            set => SetValue(ChannelImageUrlDp, value);
        }

        public static readonly DependencyProperty ChannelImageUrlDp =
            DependencyProperty.Register(
                nameof(ChannelImageUrl),
                typeof(string),
                typeof(AddCommentView),
                new PropertyMetadata((d, e) => ((AddCommentView)d)._vm.ChannelImageUrl = (string)e.NewValue)
            );

        public ICommand OnCancelCmd
        {
            get { return (ICommand)GetValue(OnCancelCmdDp); }
            set { SetValue(OnCancelCmdDp, value); }
        }

        public static readonly DependencyProperty OnCancelCmdDp =
            DependencyProperty.Register(
                nameof(OnCancelCmd),
                typeof(ICommand),
                typeof(AddCommentView),
                new PropertyMetadata((d, e) => ((AddCommentView)d)._vm.OnCancelCmd = (ICommand)e.NewValue)
            );

        public ICommand OnSubmitCmd
        {
            get { return (ICommand)GetValue(OnSubmitCmdDp); }
            set { SetValue(OnSubmitCmdDp, value); }
        }

        public static readonly DependencyProperty OnSubmitCmdDp =
            DependencyProperty.Register(
                nameof(OnSubmitCmd),
                typeof(ICommand),
                typeof(AddCommentView),
                new PropertyMetadata((d, e) => ((AddCommentView)d)._vm.OnSubmitCmd = (ICommand)e.NewValue)
            );
    }
}
