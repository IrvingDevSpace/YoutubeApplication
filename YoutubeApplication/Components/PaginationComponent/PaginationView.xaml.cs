using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.PaginationComponent
{
    /// <summary>
    /// PaginationView.xaml 的互動邏輯
    /// </summary>
    public partial class PaginationView : UserControl
    {
        public PaginationView()
        {
            InitializeComponent();

            // 內部命令：當點擊頁碼按鈕時觸發
            PageChangeCommand = new AsyncRelayCommand<int?>(
                ExecutePageChangeAsync,
                (p) => p.HasValue && p.Value != CurrentPage
            );

            PreviousPageCommand = new AsyncRelayCommand(
                () => ExecutePageChangeAsync(CurrentPage - 1),
                () => CurrentPage > 1
            );

            NextPageCommand = new AsyncRelayCommand(
                () => ExecutePageChangeAsync(CurrentPage + 1),
                () => CurrentPage < TotalPages
            );

            // 初始化顯示
            UpdatePages();
        }

        #region 外部 API (Dependency Properties)

        /// <summary>
        /// 當前頁碼
        /// </summary>
        public int CurrentPage
        {
            get => (int)GetValue(CurrentPageProperty);
            set => SetValue(CurrentPageProperty, value);
        }

        public static readonly DependencyProperty CurrentPageProperty =
            DependencyProperty.Register(
                nameof(CurrentPage),
                typeof(int),
                typeof(PaginationView),
                new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDataChanged)
            );

        /// <summary>
        /// 每頁幾筆
        /// </summary>
        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(
                nameof(PageSize),
                typeof(int),
                typeof(PaginationView),
                new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDataChanged)
            );

        /// <summary>
        /// 每頁顯示個數選擇器的選項設定
        /// </summary>
        public IEnumerable<int> PageSizes
        {
            get => (IEnumerable<int>)GetValue(PageSizesProperty);
            set => SetValue(PageSizesProperty, value);
        }

        public static readonly DependencyProperty PageSizesProperty =
            DependencyProperty.Register(
                nameof(PageSizes),
                typeof(IEnumerable<int>),
                typeof(PaginationView),
                new PropertyMetadata(new List<int> { 5, 10, 20, 50 })
            );

        /// <summary>
        /// 總筆數
        /// </summary>
        public int Total
        {
            get => (int)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }

        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register(
                nameof(Total),
                typeof(int),
                typeof(PaginationView),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnDataChanged)
            );

        // 頁碼改變事件
        public ICommand OnPageChangeCommand
        {
            get => (ICommand)GetValue(OnPageChangeCommandProperty);
            set => SetValue(OnPageChangeCommandProperty, value);
        }

        public static readonly DependencyProperty OnPageChangeCommandProperty =
            DependencyProperty.Register(
                nameof(OnPageChangeCommand),
                typeof(ICommand),
                typeof(PaginationView),
                new PropertyMetadata()
            );

        public static readonly DependencyProperty OnPageSizeChangeCommandProperty =
            DependencyProperty.Register(
                nameof(OnPageSizeChangeCommand),
                typeof(ICommand),
                typeof(PaginationView)
            );

        public ICommand OnPageSizeChangeCommand
        {
            get => (ICommand)GetValue(OnPageSizeChangeCommandProperty);
            set => SetValue(OnPageSizeChangeCommandProperty, value);
        }

        private static void OnDataChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is PaginationView control)
                control.UpdatePages();
        }

        #endregion

        /// <summary>
        /// 總頁數
        /// </summary>
        public int TotalPages => (Total > 0) ? (int)Math.Ceiling((double)Total / PageSize) : 1;

        public ObservableCollection<int> Pages { get; } = [];

        public ICommand PageChangeCommand { get; }

        public ICommand PreviousPageCommand { get; }

        public ICommand NextPageCommand { get; }

        private async Task ExecutePageChangeAsync(int? currentPage)
        {
            if (currentPage is int p)
            {
                CurrentPage = p;
                await OnPageChangeCommand.ExecuteAsync(p);
            }
        }

        /// <summary>
        /// 模仿 Element Plus 的核心摺疊邏輯
        /// </summary>
        private void UpdatePages()
        {
            int total = TotalPages;

            // 如果目前的頁數比目標總頁數多，移除多餘的
            while (Pages.Count > total)
            {
                Pages.RemoveAt(Pages.Count - 1);
            }

            // 如果目前的頁數比目標總頁數少，補上缺失的
            while (Pages.Count < total)
            {
                Pages.Add(Pages.Count + 1);
            }

            // 確保如果 total 變小導致 CurrentPage 溢出時，跳回最後一頁
            if (CurrentPage > total && total > 0)
            {
                CurrentPage = total;
            }

            //Pages.Clear();
            //int total = TotalPages;
            //int current = CurrentPage;
            //int pagerCount = 7; // 最大顯示數量

            //if (total <= pagerCount)
            //{
            //    for (int i = 1; i <= total; i++)
            //        Pages.Add(i);
            //}
            //else
            //{
            //    bool showPrevMore = current > 4;
            //    bool showNextMore = current < total - 3;

            //    // 表示還在前四頁
            //    if (!showPrevMore && showNextMore)
            //    {
            //        // 顯示前5頁
            //        for (int i = 1; i <= 5; i++)
            //            Pages.Add(i);

            //        Pages.Add(null); // 代表 "..."
            //        Pages.Add(total);
            //    }
            //    // 
            //    else if (showPrevMore && !showNextMore)
            //    {
            //        Pages.Add(1);
            //        Pages.Add(null);
            //        for (int i = total - 4; i <= total; i++)
            //            Pages.Add(i);
            //    }
            //    else
            //    {
            //        Pages.Add(1);
            //        Pages.Add(null);
            //        Pages.Add(current - 1);
            //        Pages.Add(current);
            //        Pages.Add(current + 1);
            //        Pages.Add(null);
            //        Pages.Add(total);
            //    }
            //}
        }
    }
}
