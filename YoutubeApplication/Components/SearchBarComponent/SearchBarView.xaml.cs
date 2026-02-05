using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace YoutubeApplication.Components.SearchBarComponent
{
    /// <summary>
    /// SearchBarView.xaml 的互動邏輯
    /// </summary>
    public partial class SearchBarView : UserControl
    {
        private readonly SearchBarViewModel _vm = new();

        public SearchBarView()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        public ICommand OnSearchCommand
        {
            get { return (ICommand)GetValue(OnSearchCommandProperty); }
            set { SetValue(OnSearchCommandProperty, value); }
        }

        public static readonly DependencyProperty OnSearchCommandProperty =
            DependencyProperty.Register(nameof(OnSearchCommand), typeof(ICommand), typeof(SearchBarView), new PropertyMetadata(OnSearchCommandChanged));

        private static void OnSearchCommandChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var view = (SearchBarView)d;
            view._vm.ExternalSearchCommand = e.NewValue as ICommand;
        }

        //public Func<string, Task>? OnSearchAsync
        //{
        //    get => (Func<string, Task>?)GetValue(OnSearchAsyncProperty);
        //    set => SetValue(OnSearchAsyncProperty, value);
        //}

        //public static readonly DependencyProperty OnSearchAsyncProperty =
        //    DependencyProperty.Register(
        //        nameof(OnSearchAsync),
        //        typeof(Func<string, Task>),
        //        typeof(SearchBarView),
        //        new PropertyMetadata(null, OnSearchChanged)
        //    );

        //private static void OnSearchChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    var view = (SearchBarView)d;
        //    view._vm.ExternalSearchAsync = (Func<string, Task>?)e.NewValue;
        //}
    }
}
