using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeApplication.Views.SearchFilter
{
    /// <summary>
    /// SearchFilterView.xaml 的互動邏輯
    /// </summary>
    public partial class SearchFilterView : UserControl
    {
        private SearchFilterViewModel _vm;

        public SearchFilterView()
        {
            InitializeComponent();
            _vm = new SearchFilterViewModel();
            DataContext = _vm;
        }

        public ICommand OnSubmit
        {
            get { return (ICommand)GetValue(OnSubmitProperty); }
            set { SetValue(OnSubmitProperty, value); }
        }

        public static readonly DependencyProperty OnSubmitProperty =
            DependencyProperty.Register(
                nameof(OnSubmit),
                typeof(ICommand),
                typeof(SearchFilterView),
                new PropertyMetadata((d, e) => ((SearchFilterView)d)._vm.OnSubmit = (ICommand)e.NewValue)
            );

        public ICommand OnClose
        {
            get { return (ICommand)GetValue(OnCloseProperty); }
            set { SetValue(OnCloseProperty, value); }
        }

        public static readonly DependencyProperty OnCloseProperty =
            DependencyProperty.Register(
                nameof(OnClose),
                typeof(ICommand),
                typeof(SearchFilterView),
                new PropertyMetadata((d, e) => ((SearchFilterView)d)._vm.OnClose = (ICommand)e.NewValue)
            );
    }
}
