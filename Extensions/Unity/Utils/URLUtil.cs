using UnityEngine;
using UnityEngine.Networking;

namespace Build1.PostMVC.Extensions.Unity.Utils
{
    public static class URLUtil
    {
        public static void Open(string url)
        {
            Application.OpenURL(url);
        }

        public static string Escape(string str)
        {
            return UnityWebRequest.EscapeURL(str).Replace("+","%20");
        }
    }
}