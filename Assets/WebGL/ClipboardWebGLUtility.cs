using System;
using System.Runtime.InteropServices;
using AOT;
using UnityEngine;

namespace NoTask.WebGLSupport.Clipboard
{
    static class WebGLClipboardPlugin
    {
        [DllImport("__Internal")]
        public static extern void AddEvents(Action<string> action);
        
        [DllImport("__Internal")]
        public static extern void CopyToClipboard(string text);

        [DllImport("__Internal")]
        public static extern int ClipboardContainsText();
    }

    public static class ClipboardWebGLUtility
    {
        public static event Action<string> OnCopyClipboardEvent = (text) => { };

        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
        private static void Initialize()
        {
            Debug.Log("initialize");
            WebGLClipboardPlugin.AddEvents(OnCopy);
        }
        

        public static void CopyTextToClipboard(string text)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            WebGLClipboardPlugin.CopyToClipboard(text);
#else 
            GUIUtility.systemCopyBuffer = text;
#endif
        }
        public static bool ContainsTextInClipboard()
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            return WebGLClipboardPlugin.ClipboardContainsText() == 1;
#else 
            return GUIUtility.systemCopyBuffer != null;
#endif
        }
        
        [MonoPInvokeCallback(typeof(Action<string>))]
        public static void OnCopy(string text)
        {
#if !UNITY_EDITOR && UNITY_WEBGL
            Debug.Log("test: "+text);
#else 
            Debug.Log("test: "+text);
#endif
        }
    }
}