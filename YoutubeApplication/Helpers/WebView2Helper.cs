using Microsoft.Web.WebView2.Wpf;
using System.IO;
using System.Text;
using System.Windows;

namespace YoutubeApplication.Helpers
{
    public static class WebView2Helper
    {
        // 1. 定義附加屬性，使用 nameof 避免硬編碼字串
        public static readonly DependencyProperty BindableSourceProperty =
            DependencyProperty.RegisterAttached(
                "BindableSource", // 這裡必須是屬性名稱字串，但下面我們會用 nameof 保持一致性
                typeof(string),
                typeof(WebView2Helper),
                new PropertyMetadata(null, OnBindableSourceChanged));

        // 2. 提供 Get/Set 方法（這是附加屬性的標準做法）
        public static string GetBindableSource(DependencyObject obj) => (string)obj.GetValue(BindableSourceProperty);
        public static void SetBindableSource(DependencyObject obj, string value) => obj.SetValue(BindableSourceProperty, value);

        private static async void OnBindableSourceChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            // 確保對象是 WebView2 且新值不為空
            if (d is not WebView2 webView || e.NewValue is not string videoId || string.IsNullOrWhiteSpace(videoId))
                return;

            if (webView.CoreWebView2 == null)
                await webView.EnsureCoreWebView2Async();

            if (webView.CoreWebView2 == null) return;

            // 注入 videoId
            string html = $$"""
                <!DOCTYPE html>
                <html style='height:100%; margin:0;'>
                <head>
                    <meta charset='utf-8' />
                    <style>
                        body {height:100%; margin:0; background:black; overflow:hidden; }
                        iframe {position:absolute; top:0; left:0; width:100%; height:100%; border:0; }
                    </style>
                </head>
                <body>
                    <iframe src='https://www.youtube.com/embed/{{videoId}}' 
                            allow='autoplay; encrypted-media' allowfullscreen></iframe>
                </body>
                </html>
            """;

            // 將 HTML 寫入檔案
            string tempPath = Path.Combine(Path.GetTempPath(), "YoutubeDemo");
            if (!Directory.Exists(tempPath)) Directory.CreateDirectory(tempPath);

            string fileName = "index.html";
            File.WriteAllText(Path.Combine(tempPath, fileName), html, Encoding.UTF8);

            // 設定虛擬主機映射 (解決 Error 153)
            webView.CoreWebView2.SetVirtualHostNameToFolderMapping(
                "youtube.local",
                tempPath,
                Microsoft.Web.WebView2.Core.CoreWebView2HostResourceAccessKind.Allow);

            // 導向這個偽造的網址
            webView.Source = new Uri("https://youtube.local/index.html");
        }
    }
}
