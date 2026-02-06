using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeApplication.Components.PaginationComponent
{
    /// <summary>
    /// PaginationView.xaml 的互動邏輯
    /// </summary>
    public partial class PaginationView : UserControl
    {
        private readonly PaginationViewModel _vm = new();

        public PaginationView()
        {
            InitializeComponent();
            DataContext = _vm;

            _vm.OnPageIndexChange += (index) => PageIndex = index;
            _vm.OnPageSizeChange += (size) => PageSize = size;
        }

        /// <summary>
        /// 總筆數
        /// </summary>
        public int Total
        {
            get => (int)GetValue(TotalProperty);
            set => SetValue(TotalProperty, value);
        }
        public static readonly DependencyProperty TotalProperty =
            DependencyProperty.Register(nameof(Total), typeof(int), typeof(PaginationView),
                new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (d, e) => ((PaginationView)d)._vm.Total = (int)e.NewValue));

        /// <summary>
        /// 當前index
        /// </summary>
        public int PageIndex
        {
            get => (int)GetValue(PageIndexProperty);
            set => SetValue(PageIndexProperty, value);
        }
        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register(nameof(PageIndex), typeof(int), typeof(PaginationView),
                 new FrameworkPropertyMetadata(1, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (d, e) => ((PaginationView)d)._vm.PageIndex = (int)e.NewValue));

        public ICommand OnPageIndexChangeCommand
        {
            get { return (ICommand)GetValue(OnPageIndexChangeCommandProperty); }
            set { SetValue(OnPageIndexChangeCommandProperty, value); }
        }

        public static readonly DependencyProperty OnPageIndexChangeCommandProperty =
            DependencyProperty.Register(nameof(OnPageIndexChangeCommand), typeof(ICommand), typeof(PaginationView),
                 new PropertyMetadata((d, e) => ((PaginationView)d)._vm.ExternalOnPageIndexChangeCommand = (ICommand)e.NewValue));

        /// <summary>
        /// 每頁幾筆
        /// </summary>
        public int PageSize
        {
            get => (int)GetValue(PageSizeProperty);
            set => SetValue(PageSizeProperty, value);
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register(nameof(PageSize), typeof(int), typeof(PaginationView),
               new FrameworkPropertyMetadata(5, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault,
                    (d, e) => ((PaginationView)d)._vm.PageSize = (int)e.NewValue));

        public ICommand OnPageSizeChangeCommand
        {
            get { return (ICommand)GetValue(OnPageSizeChangeCommandProperty); }
            set { SetValue(OnPageSizeChangeCommandProperty, value); }
        }

        public static readonly DependencyProperty OnPageSizeChangeCommandProperty =
            DependencyProperty.Register(nameof(OnPageSizeChangeCommand), typeof(ICommand), typeof(PaginationView),
                 new PropertyMetadata((d, e) => ((PaginationView)d)._vm.ExternalOnPageSizeChangeCommand = (ICommand)e.NewValue));
    }
}
