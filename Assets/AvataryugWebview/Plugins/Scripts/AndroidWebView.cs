using UnityEngine;
namespace com.Avataryug.WebView
{
    public class AndoridWebView
    {
#if UNITY_ANDROID
        const string pluginName = "com.avataryug.unitywebview.AvataryugWeb";

        static AndroidJavaClass _pluginClass;
        static AndroidJavaObject _pluginInstance;

        public static AndroidJavaClass PluginClass
        {
            get
            {
                if (_pluginClass == null)
                {
                    _pluginClass = new AndroidJavaClass(pluginName);
                    AndroidJavaClass playerClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                    AndroidJavaObject activity = playerClass.GetStatic<AndroidJavaObject>("currentActivity");
                    _pluginClass.SetStatic<AndroidJavaObject>("mainActivity", activity);
                }
                return _pluginClass;
            }
        }

        public static AndroidJavaObject PluginInstance
        {
            get
            {
                if (_pluginInstance == null)
                {
                    _pluginInstance = PluginClass.CallStatic<AndroidJavaObject>("getInstance");
                }
                return _pluginInstance;
            }
        }
#endif
    }
}