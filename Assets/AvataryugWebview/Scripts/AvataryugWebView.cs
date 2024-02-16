using UnityEngine;

namespace com.Avataryug.WebView
{
    public class AvataryugWebView : MonoBehaviour
    {
        public string ProjectID;
        public RectTransform webPanel;
        public RectTransform topOffset;

        private void OnEnable()
        {
            OpenWebViewTapped();
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