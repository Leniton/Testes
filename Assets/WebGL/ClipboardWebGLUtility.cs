using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace NoTask.WebGLSupport.Clipboard
{
    static class WebGLClipboardPlugin
    {
        [DllImport("__Internal")]
        public static extern void CopyToClipboard(string text);

        [DllImport("__Internal")]
        public static extern int ClipboardContainsText();
    }

    public static class ClipboardWebGLUtility
    {
        public static event Action<string> OnCopyClipboardEvent = (text) => { };

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
    }
}