#if UNITY_EDITOR

using UnityEditor;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Editor
{
    public static class AssetBundlesMenu
    {
        [MenuItem("Asset Bundles/Build", false, 10)]
        public static void Build()
        {
            AssetBundlesBuilder.Build(EditorUserBuildSettings.activeBuildTarget, BuildAssetBundleOptions.StrictMode);
        }
        
        [MenuItem("Asset Bundles/Rebuild", false, 11)]
        public static void Rebuild()
        {
            Clear();
            Build();
        }
        
        [MenuItem("Asset Bundles/Build Android", false, 50)]
        public static void BuildAndroid()
        {
            AssetBundlesBuilder.Build(BuildTarget.Android, BuildAssetBundleOptions.StrictMode);
        }
        
        [MenuItem("Asset Bundles/Build IOS", false, 51)]
        public static void BuildIOS()
        {
            AssetBundlesBuilder.Build(BuildTarget.iOS, BuildAssetBundleOptions.StrictMode);
        }
        
        [MenuItem("Asset Bundles/Build OSX", false, 100)]
        public static void BuildOSX()
        {
            AssetBundlesBuilder.Build(BuildTarget.StandaloneOSX, BuildAssetBundleOptions.StrictMode);
        }

        [MenuItem("Asset Bundles/Build Windows", false, 101)]
        public static void BuildWindows()
        {
            AssetBundlesBuilder.Build(BuildTarget.StandaloneWindows, BuildAssetBundleOptions.StrictMode);
        }
        
        [MenuItem("Asset Bundles/Build WebGL", false, 150)]
        public static void BuildWebGL()
        {
            AssetBundlesBuilder.Build(BuildTarget.WebGL, BuildAssetBundleOptions.StrictMode);
        }

        [MenuItem("Asset Bundles/Clear", false, 200)]
        public static void Clear()
        {
            AssetBundlesBuilder.Clear();
        }
    }
}

#endif