using System.Reflection;
using System.Runtime.InteropServices;

namespace WebViewCS;

public class Webview
{
    private IntPtr _handle;

    static Webview()
    {
        NativeLibrary.SetDllImportResolver(typeof(Webview).Assembly, ImportResolver);
    }

    private Webview(IntPtr handle)
    {
        _handle = handle;
    }

    public static Webview Create()
        => Create(false);

    public static Webview Create(bool debug)
        => Create(debug, IntPtr.Zero);

    public static Webview Create(bool debug, IntPtr windowHandle)
        => new (webview_create(debug ? 1 : 0, windowHandle));

    public static void Destroy(Webview webview)
        => webview_destroy(webview._handle);
    
    public static void Run(Webview webview)
        => webview_run(webview._handle);
    
    public static void Terminate(Webview webview)
        => webview_terminate(webview._handle);
    
    public static void SetTitle(Webview webview, string title)
        => webview_set_title(webview._handle, title);
    
    public static void SetSize(Webview webview, int width, int height, Hint hint)
        => webview_set_size(webview._handle, width, height, (int) hint);
    
    public static void Navigate(Webview webview, string url)
        => webview_navigate(webview._handle, url);
    
    public static void SetHtml(Webview webview, string html)
        => webview_set_html(webview._handle, html);
    
    public static void Init(Webview webview, string js)
        => webview_init(webview._handle, js);
    
    public static void Eval(Webview webview, string js)
        => webview_eval(webview._handle, js);

    public static void Bind(Webview webview, string name, Action<string, string> callback)
        => webview_bind(webview._handle, name, (id, req, _) => callback(id, req), IntPtr.Zero);
    
    public static void Unbind(Webview webview, string name)
        => webview_unbind(webview._handle, name);
    
    public static void Return(Webview webview, string seq, int status, string result)
        => webview_return(webview._handle, seq, status, result);
    
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
    private static extern IntPtr webview_get_native_handle(IntPtr window, int kind);

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
            throw new PlatformNotSupportedException();
        
        return libHandle;
    }
    
    private static bool IsArm64()
        => RuntimeInformation.ProcessArchitecture == Architecture.Arm64;
}
