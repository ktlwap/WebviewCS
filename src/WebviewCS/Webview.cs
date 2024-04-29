using System.Reflection;
using System.Runtime.InteropServices;

namespace WebViewCS;

public partial class Webview
{
    public static Webview GetApi()
    {
        NativeLibrary.SetDllImportResolver(typeof(Webview).Assembly, ImportResolver);
        return new Webview();
    }

    public WebviewHandle Create()
        => Create(false);

    public WebviewHandle Create(bool debug)
        => Create(debug, IntPtr.Zero);

    public WebviewHandle Create(bool debug, IntPtr windowHandle)
        => new (webview_create(debug ? 1 : 0, windowHandle));

    public void Destroy(WebviewHandle webview)
        => webview_destroy(webview);
    
    public void Run(WebviewHandle webview)
        => webview_run(webview);
    
    public void Terminate(WebviewHandle webview)
        => webview_terminate(webview);
    
    public void Dispatch(WebviewHandle webview, Action<IntPtr, IntPtr> callback)
        => webview_dispatch(webview, (handle, args) => callback(handle, args), IntPtr.Zero);
    
    public IntPtr GetWindow(WebviewHandle webview)
        => webview_get_window(webview);

    public IntPtr GetNativeHandle(WebviewHandle webview, NativeHandleKind kind)
        => webview_get_native_handle(webview, (int)kind);
    
    public void SetTitle(WebviewHandle webview, string title)
        => webview_set_title(webview, title);
    
    public void SetSize(WebviewHandle webview, int width, int height, Hint hint)
        => webview_set_size(webview, width, height, (int) hint);
    
    public void Navigate(WebviewHandle webview, string url)
        => webview_navigate(webview, url);
    
    public void SetHtml(WebviewHandle webview, string html)
        => webview_set_html(webview, html);
    
    public void Init(WebviewHandle webview, string js)
        => webview_init(webview, js);
    
    public void Eval(WebviewHandle webview, string js)
        => webview_eval(webview, js);

    public void Bind(WebviewHandle webview, string name, Action<string, string> callback)
        => webview_bind(webview, name, (id, req, _) => callback(id, req), IntPtr.Zero);
    
    public void Unbind(WebviewHandle webview, string name)
        => webview_unbind(webview, name);
    
    public void Return(WebviewHandle webview, string seq, int status, string result)
        => webview_return(webview, seq, status, result);
    
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
