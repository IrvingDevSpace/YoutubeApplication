using System.Windows;
using YoutubeApplication.Context;
using YoutubeApplication.Navigation;
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
        public static INavService NavService { get; set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            NavService = new NavService();

            var context = YoutubeContextProvider.Context;
            var mainVm = new MainWindowViewModel();

            void GoToHome()
            {
                var homeVm = new HomeViewModel();
                mainVm.CurrentVm = homeVm;
                mainVm.WindowTitle = "YouTube - 首頁";
            }

            if (context.Auth.IsLoggedIn())
                GoToHome();
            else
            {
                var loginPresenter = new LoginPresenter(context);
                var loginVm = new LoginViewModel(loginPresenter);

                loginVm.OnLoginSuccess += GoToHome;

                mainVm.CurrentVm = loginVm;
                mainVm.WindowTitle = "YouTube - 登入";
            }


            var mainWindow = new MainWindow { DataContext = mainVm };
            mainWindow.Show();
        }
    }
}
