using System.Windows.Input;

namespace YoutubeApplication.Common
{
    public interface IAsyncCommand : ICommand
    {
        Task ExecuteAsync(object? parameter);
    }

    public class RelayCommand : RelayCommand<object>
    {
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : base(_ => execute(), canExecute == null ? null : _ => canExecute()) { }
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
            //if (parameter is T t) return _canExecute(t);
            //return false;
            return _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            //if (parameter is T t) _execute(t);
            _execute((T)parameter);
        }
    }

    public class AsyncRelayCommand : AsyncRelayCommand<object>
    {
        public AsyncRelayCommand(Func<Task> execute, Func<bool>? canExecute = null)
            : base(_ => execute(), canExecute == null ? null : _ => canExecute()) { }
    }

    public class AsyncRelayCommand<T> : IAsyncCommand
    {
        private readonly Func<T, Task> _execute;
        private readonly Func<T, bool>? _canExecute;

        public AsyncRelayCommand(Func<T, Task> execute, Func<T, bool>? canExecute = null)
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
            //if (parameter is T t) return _canExecute(t);
            //return false;

            return _canExecute((T)parameter);
        }

        public void Execute(object? parameter)
        {
            _execute((T)parameter);
        }

        public async Task ExecuteAsync(object? parameter)
        {
            await _execute((T)parameter);
        }
    }
}
