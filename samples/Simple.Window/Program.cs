using WebViewCS;

namespace Simple.Window;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Webview webview = Webview.Create();
        Webview.SetSize(webview, 800, 600, Hint.None);
        Webview.SetTitle(webview, "Simple.Window");
        Webview.Navigate(webview, "https://google.com");
        Webview.Run(webview);
    }
}
