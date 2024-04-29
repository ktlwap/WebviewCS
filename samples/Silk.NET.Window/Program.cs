using Silk.NET.Maths;
using Silk.NET.Windowing;
using WebViewCS;

namespace Silk.NET.Window;

class Program
{
    private static IWindow? _window;
    private static Webview? _webviewApi;
    private static WebviewHandle? _webviewHandle;

    [STAThread]
    static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Silk.NET.Window";

        _window = Windowing.Window.Create(options);
        _window.Initialize();
        _window.Load += OnLoad;
        
        _webviewApi = Webview.GetApi();
        
        _window.Run();
        _window.Dispose();
    }

    private static void OnLoad()
    {
        _webviewHandle = _webviewApi!.Create(true, _window!.Handle);
        _webviewApi.Navigate(_webviewHandle.Value, "https://google.com");
        _webviewApi.Run(_webviewHandle.Value);
    }
}
