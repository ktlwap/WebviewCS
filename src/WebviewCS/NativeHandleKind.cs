namespace WebViewCS;

public enum NativeHandleKind
{
    /**
     * Top-level window. GtkWindow pointer (GTK), NSWindow pointer (Cocoa) or HWND (Win32).
     */
    UiWindow,
    /**
     * Browser widget. GtkWidget pointer (GTK), NSView pointer (Cocoa) or HWND (Win32).
     */
    UiWidget,
    /**
     * Browser controller. WebKitWebView pointer (WebKitGTK), WKWebView pointer (Cocoa/WebKit) or
     * ICoreWebView2Controller pointer (Win32/WebView2).
     */
    BrowserController,
}
