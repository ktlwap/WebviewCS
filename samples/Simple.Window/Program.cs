using WebViewCS;

namespace Simple.Window;

class Program
{
    [STAThread]
    static void Main(string[] args)
    {
        Webview webviewApi = Webview.GetApi();
        WebviewHandle webviewHandle = webviewApi.Create();
        webviewApi.SetSize(webviewHandle, 800, 600, Hint.None);
        webviewApi.SetTitle(webviewHandle, "Simple.Window");
        webviewApi.Navigate(webviewHandle, "https://google.com");
        webviewApi.Run(webviewHandle);
    }
}
