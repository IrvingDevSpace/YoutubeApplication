using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Net;
using System.Text;
using System.Windows;

namespace YoutubeApplication.Helpers
{
    public static class WebView2Helper
    {
        private const int MaxRetryCount = 3;
        private static readonly Dictionary<Microsoft.Web.WebView2.Wpf.WebView2, int> _retryTracker = new();

        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached(
                "BindableSource",
                typeof(string),
                typeof(WebView2Helper),
                new PropertyMetadata(null, OnBindableSourceChanged));

        public static string GetBindableSource(DependencyObject obj) => (string)obj.GetValue(BindableSourceProperty);
        public static void SetBindableSource(DependencyObject obj, string value) => obj.SetValue(BindableSourceProperty, value);

        // 用一個靜態變數記錄 HTML 是否已經寫入過，避免重複寫入硬碟
        private static bool _isHtmlGenerated = false;
        private static readonly string TempPath = Path.Combine(Path.GetTempPath(), "YoutubeDemo");
        private static HttpListener Listener;

        private static async void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is not WebView2 webView || e.NewValue is not string videoId || string.IsNullOrWhiteSpace(videoId))
                return;

            if (webView.CoreWebView2 == null)
            {
                webView.Unloaded -= WebView_Unloaded; // 避免重複掛載
                webView.Unloaded += WebView_Unloaded;
                await webView.EnsureCoreWebView2Async();
            }

            if (webView.CoreWebView2 == null) return;

            // 1. 準備共用的 HTML (整個應用程式生命週期只需寫入一次)
            //if (!_isHtmlGenerated)
            //{
            if (!Directory.Exists(TempPath)) Directory.CreateDirectory(TempPath);

            // 這裡的重點是加入了一段 JS，用來接收網址列的參數
            string html = $$"""
                    <iframe width="100%" height="100%" src="https://www.youtube.com/embed/{{videoId}}"
                    frameborder="0"
                    allow="accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture; web-share"
                    referrerpolicy="strict-origin-when-cross-origin" allowfullscreen>
                    </iframe>
                """;


            if (Listener == null)
            {
                Listener = new HttpListener();
                string url = "http://localhost:5000/";
                Listener.Prefixes.Add(url);
                Listener.Start();
                webView.CoreWebView2.Navigate(url);
                HttpListenerContext context = await Listener.GetContextAsync();
                var response = context.Response;
                byte[] buffer = Encoding.UTF8.GetBytes(html);
                response.ContentLength64 = buffer.Length;
                response.OutputStream.Write(buffer, 0, buffer.Length);
                response.OutputStream.Close();
                Listener.Stop();
                Listener = null;
            }

            //File.WriteAllText(Path.Combine(TempPath, "player.html"), html, Encoding.UTF8);
            //    _isHtmlGenerated = true;
            //}

            //// 2. 註冊虛擬主機 (確保這個 WebView2 實例有註冊到)
            //webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
            //    "youtube.local",
            //    TempPath,
            //    Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);

            //// 3. 直接導向帶有參數的虛擬網址，只做網址跳轉！
            //webView.Source = new Uri($"https://youtube.local/player.html?v={videoId}");
        }


        private static void WebView_Unloaded(object sender, RoutedEventArgs e)
        {
            if (sender is WebView2 webView)
            {
                // 方法 A：導向空白頁（最溫和，下次切換回來時較快）
                webView.Source = new Uri("about:blank");

                // 方法 B：直接釋放資源（最徹底，但下次載入需重新初始化）
                // webView.Dispose(); 
            }
        }
    }
}
