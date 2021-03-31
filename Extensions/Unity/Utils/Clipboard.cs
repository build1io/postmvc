using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Utils
{
    public static class Clipboard
    {
        public static void Paste(string content)
        {
            GUIUtility.systemCopyBuffer = content;
        }

        public static void CopyToClipboard(this string content)
        {
            GUIUtility.systemCopyBuffer = content;
        }
    }
}