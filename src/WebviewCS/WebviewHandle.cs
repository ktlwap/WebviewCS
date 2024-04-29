namespace WebViewCS;

public struct WebviewHandle
{
    public readonly IntPtr Handle;
    
    public WebviewHandle(IntPtr handle)
    {
        Handle = handle;
    }
}