using System.Reflection;
using System.Runtime.InteropServices;

namespace WebViewCS;

public class WebView
{
    private IntPtr _handle;

    static WebView()
    {
        NativeLibrary.SetDllImportResolver(typeof(WebView).Assembly, ImportResolver);
    }

    private WebView(IntPtr handle)
    {
        _handle = handle;
    }

    public static WebView Create()
        => Create(false);

    public static WebView Create(bool debug)
        => Create(debug, IntPtr.Zero);

    public static WebView Create(bool debug, IntPtr windowHandle)
        => new (webview_create(debug ? 1 : 0, windowHandle));

    public static void Destroy(WebView webView)
        => webview_destroy(webView._handle);
    
    public static void Run(WebView webView)
        => webview_run(webView._handle);
    
    public static void Terminate(WebView webView)
        => webview_terminate(webView._handle);
    
    public static void SetTitle(WebView webView, string title)
        => webview_set_title(webView._handle, title);
    
    public static void SetSize(WebView webView, int width, int height, Hint hint)
        => webview_set_size(webView._handle, width, height, (int) hint);
    
    public static void Navigate(WebView webView, string url)
        => webview_navigate(webView._handle, url);
    
    public static void SetHtml(WebView webView, string html)
        => webview_set_html(webView._handle, html);
    
    public static void Init(WebView webView, string js)
        => webview_init(webView._handle, js);
    
    public static void Eval(WebView webView, string js)
        => webview_eval(webView._handle, js);

    public static void Bind(WebView webView, string name, Action<string, string> callback)
        => webview_bind(webView._handle, name, (id, req, _) => callback(id, req), IntPtr.Zero);
    
    public static void Unbind(WebView webView, string name)
        => webview_unbind(webView._handle, name);
    
    public static void Return(WebView webView, string seq, int status, string result)
        => webview_return(webView._handle, seq, status, result);
    
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DispatchFunction(
        IntPtr webview,
        IntPtr args);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void CallBackFunction(
        [MarshalAs(UnmanagedType.LPStr)] string id,
        [MarshalAs(UnmanagedType.LPStr)] string req,
        IntPtr arg);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_create(int debug, IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_destroy(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_run(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_terminate(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_dispatch(IntPtr webview, DispatchFunction dispatchFunction, IntPtr args);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_get_window(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_title(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string title);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_size(IntPtr window, int width, int height, int hints);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_navigate(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string url);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_html(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string html);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_init(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_eval(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_bind(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string name,
        CallBackFunction callback, IntPtr arg);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_unbind(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_return(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string seq, int status,
        [MarshalAs(UnmanagedType.LPStr)] string result);
    
    private static IntPtr ImportResolver(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
    {
        if (libraryName != "webview")
            return IntPtr.Zero;
        
        IntPtr libHandle = IntPtr.Zero;
        if (OperatingSystem.IsWindows())
        {
            if (Environment.Is64BitProcess)
                NativeLibrary.TryLoad("./runtimes/win-x64/native/webview.dll", assembly, searchPath,  out libHandle);
            else
                NativeLibrary.TryLoad("./runtimes/win-x86/native/webview.dll", assembly, searchPath,  out libHandle);
        }
        else if (OperatingSystem.IsMacOS())
        {
            if (IsArm64())
                NativeLibrary.TryLoad("./runtimes/osx-arm64/native/libwebview.dylib", assembly, searchPath, out libHandle);
            else
                NativeLibrary.TryLoad("./runtimes/osx-x64/native/libwebview.dylib", assembly, searchPath, out libHandle);
        }
        else if (OperatingSystem.IsLinux())
        {
            if (IsArm64())
                NativeLibrary.TryLoad("./runtimes/linux-arm64/native/libwebview.so", assembly, searchPath, out libHandle);
            else
                NativeLibrary.TryLoad("./runtimes/linux-x64/native/libwebview.so", assembly, searchPath, out libHandle);
        }

        if (libHandle == IntPtr.Zero)
            throw new Exception("Platform not found.");
        
        return libHandle;
    }
    
    private static bool IsArm64()
        => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
