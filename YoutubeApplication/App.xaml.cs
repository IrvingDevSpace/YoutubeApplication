using System.Windows;
using YoutubeApplication.Context;
using YoutubeApplication.Presenters;
using YoutubeApplication.Views.Home;
using YoutubeApplication.Views.Login;

namespace YoutubeApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var context = YoutubeContextProvider.Context;

            var mainVM = new MainWindowViewModel();

            void GoToHome()
            {
                var homePresenter = new HomePresenter(context);
                mainVM.CurrentPage = new HomeViewModel(homePresenter);
                mainVM.WindowTitle = "YouTube - 首頁";
            }

            if (!context.Auth.IsLoggedIn())
            {
                var loginPresenter = new LoginPresenter(context);
                var loginVM = new LoginViewModel(loginPresenter);

                loginVM.OnLoginSuccess += GoToHome;

                mainVM.CurrentPage = loginVM;
                mainVM.WindowTitle = "YouTube - 登入";
            }
            else
                GoToHome();

            var mainWindow = new MainWindow { DataContext = mainVM };
            mainWindow.Show();
        }
    }
}
