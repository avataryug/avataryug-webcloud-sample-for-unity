using UnityEngine;

namespace com.Avataryug.WebView
{
    public class AvataryugWebView : MonoBehaviour
    {
        public string ProjectID;
        public RectTransform webPanel;
        public RectTransform topOffset;

        const string pluginName = "com.avataryug.unitywebview.AvataryugWeb";

        class NetworkMessageReceiveCallback : AndroidJavaProxy
        {
            System.Action<string> OnReceiveNetworkMessage;
            public NetworkMessageReceiveCallback(System.Action<string> netwrtorkmsg) : base(pluginName + "$NetworkMessageReceiveCallback")
            {
                OnReceiveNetworkMessage = netwrtorkmsg;
            }
            public void onReceiveMessage(string message)
            {
                if (OnReceiveNetworkMessage != null)
                    OnReceiveNetworkMessage(message);
            }
        }

        System.Action<string> OnReceiveNetworkMessage;
        private void OnEnable()
        {
            OpenWebViewTapped();
            OnReceiveNetworkMessage += OnReceive;
#if UNITY_ANDROID
            AndoridWebView.PluginInstance.Call("SubscribeCallback", new object[] { new NetworkMessageReceiveCallback(OnReceiveNetworkMessage) });
#endif
        }

        void OnReceive(string message)
        {
            Debug.Log("=============>>>" + message);
        }

        public void OpenWebViewTapped()
        {
            Canvas parentCanvas = topOffset.GetComponentInParent<Canvas>();
            int stripHeight = (int)(topOffset.rect.height * parentCanvas.scaleFactor + 0.5f);
            webPanel.gameObject.SetActive(true);

            OpenWebView("https://" + ProjectID + ".avataredge.net", stripHeight);
        }

        public void OpenWebView(string url, int pixelShift)
        {

#if UNITY_ANDROID
            AndoridWebView.PluginInstance.Call("showWebView", new object[] { url, pixelShift });
#endif

#if UNITY_IOS
            IOSWebView.IOSClearCache();
            IOSWebView.IOSshowWebView(url, pixelShift);
#endif
        }

        public void CloseWebView(System.Action closeComplete)
        {
#if UNITY_ANDROID
            AndoridWebView.PluginInstance.Call("closeWebView");
#endif

#if UNITY_IOS
            IOSWebView.IOShideWebView();
#endif
            closeComplete();
        }

        public void CloseWebViewTapped()
        {
            CloseWebView(() =>
            {
                webPanel.gameObject.SetActive(false);
            });
        }
    }
}