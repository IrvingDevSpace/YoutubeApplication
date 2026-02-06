using PropertyChanged;
using System.Windows.Input;
using YoutubeApplication.Common;

namespace YoutubeApplication.Components.PaginationComponent
{
    [AddINotifyPropertyChangedInterface]
    public class PaginationViewModel
    {
        private readonly int _basePageSize = 5;

        public int Total { get; set; } = 0;

        public int PageIndex { get; set; } = 1;

        public void OnPageIndexChanged()
        {
            OnPageIndexChange?.Invoke(PageIndex);
        }

        public int PageSize { get; set; }

        public void OnPageSizeChanged()
        {
            OnPageSizeChange?.Invoke(PageSize);
        }

        public int TotalPageCount => Total == 0 ? 1 : (int)Math.Ceiling((double)Total / PageSize);

        public List<int> Pages => Enumerable.Range(1, TotalPageCount).ToList();

        public List<int> PageSizeOptions
        {
            get
            {
                if (Total <= 0) return [_basePageSize];
                var options = new HashSet<int>();

                for (int i = _basePageSize; i <= Total; i += _basePageSize)
                    options.Add(i);

                options.Add(Total);
                return options.OrderBy(x => x).ToList();
            }
        }

        public ICommand PreviousPageCommand { get; }

        public ICommand NextPageCommand { get; }

        public ICommand SetPageIndexCommand { get; }

        public event Action<int>? OnPageIndexChange;

        public ICommand? ExternalOnPageIndexChangeCommand { get; set; }

        public ICommand SetPageSizeCommand { get; }

        public event Action<int>? OnPageSizeChange;

        public ICommand? ExternalOnPageSizeChangeCommand { get; set; }

        public PaginationViewModel()
        {
            PageSize = _basePageSize;

            PreviousPageCommand = new AsyncRelayCommand(() => SetPageIndex(PageIndex - 1), () => PageIndex > 1);
            NextPageCommand = new AsyncRelayCommand(() => SetPageIndex(PageIndex + 1), () => PageIndex < TotalPageCount);
            SetPageIndexCommand = new AsyncRelayCommand<int>(SetPageIndex);
            SetPageSizeCommand = new AsyncRelayCommand<int>(SetPageSizeAsync);
        }

        private async Task SetPageIndex(int index)
        {
            if (index < 1 || index > TotalPageCount) return;
            PageIndex = index;

            await ExternalOnPageIndexChangeCommand.ExecuteAsync(index);
        }

        private async Task SetPageSizeAsync(int size)
        {
            if (size <= 0) return;
            PageSize = size;

            if (PageIndex > TotalPageCount)
                PageIndex = TotalPageCount;

            await ExternalOnPageSizeChangeCommand.ExecuteAsync(size);
        }
    }
}
