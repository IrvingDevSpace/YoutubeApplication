using System.Windows.Controls;

namespace YoutubeApplication.Views.Home
{
    /// <summary>
    /// HomeView.xaml 的互動邏輯
    /// </summary>
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            //InitializeWebView();
            App.NavService.Frame = frame;
        }

        async void InitializeWebView()
        {
            //// 1. 確保初始化
            //await myWebView.EnsureCoreWebView2Async();

            //// 2. 準備 HTML 內容 (嵌入 YouTube 的語法)
            //string html = """
            //    <!DOCTYPE html>
            //    <html style='height:100%; margin:0;'>
            //    <head>
            //        <meta charset='utf-8' />
            //        <style>
            //            body { height:100%; margin:0; background:black; overflow:hidden; }
            //            iframe { position:absolute; top:0; left:0; width:100%; height:100%; border:0; }
            //        </style>
            //    </head>
            //    <body>
            //        <iframe src='https://www.youtube.com/embed/yzpMHLHEm6I' 
            //                allow='autoplay; encrypted-media' allowfullscreen></iframe>
            //    </body>
            //    </html>";
            //""";

            //// 3. 建立暫存資料夾
            //string tempPath = Path.Combine(Path.GetTempPath(), "YoutubeDemo");
            //if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            //// 4. 將 HTML 寫入檔案
            //string fileName = "index.html";
            //File.WriteAllText(Path.Combine(tempPath, fileName), html, Encoding.UTF8);

            //// 5. 設定虛擬主機映射 (這步最關鍵，解決 Error 153)
            //myWebView.CoreWebView2.SetVirtualHostNameToFolderMapping(
            //    "youtube.local",
            //    tempPath,
            //    Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);

            //// 6. 導向這個偽造的網址
            //myWebView.Source = new Uri("https://youtube.local/index.html");
        }
    }
}
