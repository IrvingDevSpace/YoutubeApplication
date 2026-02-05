using System.Windows.Input;

namespace YoutubeApplication.Common
{
    //public class RelayCommand<T> : ICommand
    //{
    //    private readonly Action<T?> _execute;
    //    private readonly Predicate<T?>? _canExecute;

    //    public RelayCommand(Action<T?> execute, Predicate<T?>? canExecute = null)
    //    {
    //        _execute = execute ?? throw new ArgumentNullException(nameof(execute));
    //        _canExecute = canExecute;
    //    }

    //    // 掛載到 WPF 的 CommandManager，讓 UI 自動偵測 CanExecute 變化
    //    public event EventHandler? CanExecuteChanged
    //    {
    //        add => CommandManager.RequerySuggested += value;
    //        remove => CommandManager.RequerySuggested -= value;
    //    }

    //    public bool CanExecute(object? parameter)
    //    {
    //        if (parameter == null && typeof(T).IsValueType) return false;
    //        return _canExecute == null || _canExecute((T?)parameter);
    //    }

    //    public void Execute(object? parameter)
    //    {
    //        _execute((T?)parameter);
    //    }
    //}

    //public class RelayCommand : RelayCommand<object>
    //{
    //    public RelayCommand(Action execute, Func<bool>? canExecute = null)
    //        : base(_ => execute(), canExecute == null ? null : _ => canExecute()) { }
    //}

    public class RelayCommand : ICommand
    {
        private readonly Func<bool>? _canExecute;
        private readonly Action _execute;

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute == null || _canExecute();

        public void Execute(object? parameter) => _execute();
    }


    public class RelayCommand<T> : ICommand
    {
        private readonly Action<T> _execute;
        private readonly Func<T, bool>? _canExecute;

        public RelayCommand(Action<T> execute, Func<T, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecute == null) return true;
            if (parameter is T t) return _canExecute(t);
            return false;
        }

        public void Execute(object? parameter)
        {
            if (parameter is T t) _execute(t);
        }
    }
}
