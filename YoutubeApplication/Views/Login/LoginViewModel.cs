using System.Diagnostics;
using System.Windows.Input;
using YoutubeApplication.Common;
using YoutubeApplication.Presenters.Interfaces;

namespace YoutubeApplication.Views.Login
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly ILoginPresenter _presenter;

        public event Action OnLoginSuccess;

        public ICommand LoginCommand { get; }

        public LoginViewModel(ILoginPresenter presenter)
        {
            _presenter = presenter;

            LoginCommand = new RelayCommand(
                execute: async () => await ExecuteLoginAsync(),
                canExecute: () => !IsLoading
            );
        }

        private async Task ExecuteLoginAsync()
        {
            IsLoading = true;

            var result = await _presenter.LoginAsync();

            if (result.IsSuccess)
                OnLoginSuccess?.Invoke();

            Debug.WriteLine(result.Message);

            IsLoading = false;
        }
    }
}
