#if UNITY_EDITOR

using UnityEditor;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Editor
{
    [InitializeOnLoad]
    public static class AssetsRefreshTool
    {
        public const string AutoRefreshOnPlayPrefsKey = "PostMVC_RefreshAssetsOnPlay";
        
        static AssetsRefreshTool()
        {
            SetEnabled(GetEnabled(), false);
        }

        public static bool GetEnabled()
        {
            return EditorPrefs.GetBool(AutoRefreshOnPlayPrefsKey);
        }
        
        public static void SetEnabled(bool enabled, bool printToLog = true)
        {
            EditorPrefs.SetBool("kAutoRefresh", !enabled);
            EditorPrefs.SetBool("PostMVC_RefreshAssetsOnPlay", enabled);
            
            if (enabled)
                EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            else
                EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            
            if (!printToLog)
                return;
            
            Debug.Log(enabled
                          ? "Refresh Assets on Play Mode enabled. Auto Refresh turned off."
                          : "Refresh Assets on Play Mode disabled. Auto Refresh turned on.");
        }
        
        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state != PlayModeStateChange.ExitingEditMode) 
                return;
            Debug.Log("Refreshing assets...");
            AssetDatabase.Refresh(ImportAssetOptions.Default);
        }
    }
}

#endif