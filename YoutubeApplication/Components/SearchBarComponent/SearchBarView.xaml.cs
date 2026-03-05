using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.SearchBarComponent
{
    /// <summary>
    /// SearchBarView.xaml 的互動邏輯
    /// </summary>
    public partial class SearchBarView : UserControl
    {
        public SearchBarView()
        {
            InitializeComponent();
            SearchCommand = new AsyncRelayCommand(ExecuteSearch, CanExecuteSearch);
        }

        #region 外部 API (Dependency Properties)

        /// <summary>
        /// 搜尋關鍵字
        /// </summary>
        public string Keyword
        {
            get => (string)GetValue(KeywordProperty);
            set => SetValue(KeywordProperty, value);
        }

        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register(
                nameof(Keyword),
                typeof(string),
                typeof(SearchBarView),
                new FrameworkPropertyMetadata("", FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        /// <summary>
        /// 是否處於搜尋狀態
        /// </summary>
        public bool IsSearching
        {
            get => (bool)GetValue(IsSearchingProperty);
            set => SetValue(IsSearchingProperty, value);
        }

        public static readonly DependencyProperty IsSearchingProperty =
            DependencyProperty.Register(
                nameof(IsSearching),
                typeof(bool),
                typeof(SearchBarView),
                new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault)
            );

        /// <summary>
        /// 搜尋事件
        /// </summary>
        public ICommand OnSearchCommand
        {
            get { return (ICommand)GetValue(OnSearchCommandProperty); }
            set { SetValue(OnSearchCommandProperty, value); }
        }

        public static readonly DependencyProperty OnSearchCommandProperty =
            DependencyProperty.Register(
                nameof(OnSearchCommand),
                typeof(ICommand),
                typeof(SearchBarView),
                new PropertyMetadata()
            );

        #endregion

        public ICommand SearchCommand { get; }

        private bool CanExecuteSearch()
        {
            return !string.IsNullOrWhiteSpace(Keyword) &&
                   !IsSearching &&
                   (OnSearchCommand?.CanExecute(Keyword) ?? true);
        }

        private async Task ExecuteSearch()
        {
            if (IsSearching) return;

            IsSearching = true;
            try
            {
                await OnSearchCommand.ExecuteAsync(Keyword);
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}
