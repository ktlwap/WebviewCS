using WebViewCS;

namespace Simple.Window;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        WebView webView = WebView.Create();
        WebView.SetTitle(webView, "Simple.Window");
        WebView.Navigate(webView, "https://google.at");
        WebView.Run(webView);
    }
}
