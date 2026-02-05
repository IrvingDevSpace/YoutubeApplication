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
            SearchCommand = new RelayCommand(
                execute: () =>
                {
                    if (IsSearching) return;

                    IsSearching = true;
                    try
                    {
                        ExternalSearchCommand?.Execute(Keyword);

                        //if (ExternalSearchAsync != null)
                        //    await ExternalSearchAsync(Keyword);
                    }
                    finally
                    {
                        IsSearching = false;
                    }
                },
                canExecute: () => !string.IsNullOrWhiteSpace(Keyword) && !IsSearching
            );
        }
    }
}
