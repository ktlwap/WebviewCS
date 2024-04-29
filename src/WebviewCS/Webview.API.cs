using System.Runtime.InteropServices;

namespace WebViewCS;

public partial class Webview
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void DispatchFunction(
        IntPtr webview,
        IntPtr args
    );

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    private delegate void CallBackFunction(
        [MarshalAs(UnmanagedType.LPStr)] string id,
        [MarshalAs(UnmanagedType.LPStr)] string req,
        IntPtr arg
    );

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_create(int debug, IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_destroy(WebviewHandle window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_run(WebviewHandle window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_terminate(WebviewHandle handle);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_dispatch(WebviewHandle handle, DispatchFunction dispatchFunction, IntPtr args);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_get_window(WebviewHandle handle);
    
    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_get_native_handle(WebviewHandle handle, int kind);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_title(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string title);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_size(WebviewHandle handle, int width, int height, int hints);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_navigate(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string url);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_set_html(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string html);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_init(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_eval(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern void webview_bind(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string name,
        CallBackFunction callback, IntPtr arg);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_unbind(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    private static extern IntPtr webview_return(WebviewHandle handle, [MarshalAs(UnmanagedType.LPStr)] string seq, int status,
        [MarshalAs(UnmanagedType.LPStr)] string result);
}