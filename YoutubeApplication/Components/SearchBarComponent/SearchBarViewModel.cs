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

        public event Func<string, Task>? OnSearchRequestedAsync;

        public ICommand SearchCommand { get; }

        public SearchBarViewModel()
        {
            SearchCommand = new RelayCommand(
                execute: async () =>
                {
                    if (IsSearching) return;

                    try
                    {
                        IsSearching = true;

                        if (OnSearchRequestedAsync != null)
                            await OnSearchRequestedAsync.Invoke(Keyword.Trim());
                    }
                    finally
                    {
                        IsSearching = false; // 3. 搜尋結束，按鈕恢復可用
                    }
                },
                canExecute: () => !string.IsNullOrWhiteSpace(Keyword) && !IsSearching
            );
        }
    }
}
