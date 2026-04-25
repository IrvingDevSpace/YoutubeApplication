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
            SubmitCommand = new AsyncRelayCommand(Execute, CanExecute);
        }

        #region 外部 API (Dependency Properties)

        /// <summary>
        /// 搜尋關鍵字
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
        /// 搜尋事件
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

        #endregion

        public ICommand SubmitCommand { get; }

        private bool CanExecute()
        {
            return !string.IsNullOrWhiteSpace(Content) &&
                   (OnSubmitCommand?.CanExecute(Content) ?? true);
        }

        private async Task Execute()
        {
            await OnSubmitCommand.ExecuteAsync(Content);
            Content = "";
        }
    }
}
