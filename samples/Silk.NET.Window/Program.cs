using Silk.NET.Maths;
using Silk.NET.Windowing;
using Silk.NET.Windowing.Glfw;
using WebViewCS;

namespace Silk.NET.Window;

class Program
{
    private static IWindow _window;
    private static WebView _webView;

    [STAThread]
    static void Main(string[] args)
    {
        WindowOptions options = WindowOptions.Default;
        options.Size = new Vector2D<int>(800, 600);
        options.Title = "Silk.NET.Window";

        _window = Silk.NET.Windowing.Window.Create(options);
        _window.Initialize();
        _window.Load += OnLoad;
        
        _webView = WebView.Create(true, _window.Handle);
        
        _window.Run();
        _window.Dispose();
    }

    private static void OnLoad()
    {
        WebView.SetTitle(_webView, "Simple.Window");
        WebView.Navigate(_webView, "https://google.at");
        WebView.Run(_webView);
    }
}
