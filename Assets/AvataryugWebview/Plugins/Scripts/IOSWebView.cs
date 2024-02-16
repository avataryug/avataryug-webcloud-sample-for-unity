using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

namespace com.Avataryug.WebView
{
    public class IOSWebView
    {

#if UNITY_IOS

    [DllImport("__Internal")]
    public static extern void IOSshowWebView(string URL, int pixelSpace);

    [DllImport("__Internal")]
    public static extern void IOShideWebView();

    [DllImport("__Internal")]
    public static extern void IOSClearCache();

#endif
    }
}