using PropertyChanged;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.SearchBarComponent
{
    [AddINotifyPropertyChangedInterface]
    public class SearchBarViewModel
    {
        public string Keyword { get; set; } = "";

        public bool IsSearching { get; set; }

        public ICommand SearchCommand { get; }

        public ICommand? ExternalSearchCommand { get; set; }

        //public Func<string, Task>? ExternalSearchAsync { get; set; }

        public SearchBarViewModel()
        {
            SearchCommand = new AsyncRelayCommand(
                execute: ExecuteSearch,
                canExecute: CanExecuteSearch
            );
        }

        private bool CanExecuteSearch()
        {
            return !string.IsNullOrWhiteSpace(Keyword) &&
                   !IsSearching &&
                   (ExternalSearchCommand?.CanExecute(Keyword) ?? true);
        }

        private async Task ExecuteSearch()
        {
            if (IsSearching) return;

            IsSearching = true;
            try
            {
                await ExternalSearchCommand.ExecuteAsync(Keyword);
            }
            finally
            {
                IsSearching = false;
            }
        }
    }
}
