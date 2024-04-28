using WebViewCS;

namespace Simple.Window;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        WebView webView = WebView.Create();
        WebView.SetSize(webView, 800, 600, Hint.None);
        WebView.SetTitle(webView, "Simple.Window");
        WebView.Navigate(webView, "https://google.com");
        WebView.Run(webView);
    }
}
