using System.Runtime.InteropServices;

namespace WebView;

public class WebView
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void DispatchFunction(
        IntPtr webview,
        IntPtr args);

    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    internal delegate void CallBackFunction(
        [MarshalAs(UnmanagedType.LPStr)] string id,
        [MarshalAs(UnmanagedType.LPStr)] string req,
        IntPtr arg);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_create(int debug, IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void webview_destroy(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void webview_run(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void webview_terminate(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void webview_dispatch(IntPtr webview, DispatchFunction dispatchFunction, IntPtr args);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_get_window(IntPtr window);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_set_title(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string title);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_set_size(IntPtr window, int width, int height, int hints);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_navigate(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string url);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_set_html(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string html);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_init(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_eval(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string js);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern void webview_bind(IntPtr webview, [MarshalAs(UnmanagedType.LPStr)] string name,
        CallBackFunction callback, IntPtr arg);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_unbind(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string name);

    [DllImport("webview", CallingConvention = CallingConvention.Cdecl)]
    internal static extern IntPtr webview_return(IntPtr window, [MarshalAs(UnmanagedType.LPStr)] string seq, int status,
        [MarshalAs(UnmanagedType.LPStr)] string result);
}
