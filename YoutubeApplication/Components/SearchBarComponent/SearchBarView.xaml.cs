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

            SearchCommand = new AsyncRelayCommand(
                execute: ExecuteSearch, canExecute: CanExecuteSearch
            );
        }

        public string Keyword
        {
            get => (string)GetValue(KeywordProperty);
            set => SetValue(KeywordProperty, value);
        }

        public static readonly DependencyProperty KeywordProperty =
            DependencyProperty.Register(nameof(Keyword), typeof(string), typeof(SearchBarView),
                new FrameworkPropertyMetadata(""));

        public bool IsSearching
        {
            get => (bool)GetValue(IsSearchingProperty);
            set => SetValue(IsSearchingProperty, value);
        }

        public static readonly DependencyProperty IsSearchingProperty =
            DependencyProperty.Register(nameof(IsSearching), typeof(bool), typeof(SearchBarView), new PropertyMetadata(false));

        public ICommand OnSearchCommand
        {
            get { return (ICommand)GetValue(OnSearchCommandProperty); }
            set { SetValue(OnSearchCommandProperty, value); }
        }

        public readonly DependencyProperty OnSearchCommandProperty =
            DependencyProperty.Register(nameof(OnSearchCommand), typeof(ICommand), typeof(SearchBarView),
                 new PropertyMetadata((d, e) => ((SearchBarView)d).OnSearchCommand = (ICommand)e.NewValue));

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
