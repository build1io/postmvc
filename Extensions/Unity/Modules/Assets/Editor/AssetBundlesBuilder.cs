#if UNITY_EDITOR

using System.IO;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEditor;
using UnityEditor.Build;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Editor
{
    public class AssetBundlesBuilder : IActiveBuildTargetChanged
    {
        private static readonly ILogger Logger = LoggerProvider.GetLogger<AssetBundlesBuilder>(LogLevel.All);

        private const string AssetBundlesDirectory = "Assets/StreamingAssets";

        public int callbackOrder { get; }

        public void OnActiveBuildTargetChanged(BuildTarget previousTarget, BuildTarget newTarget)
        {
            Logger.Debug("Current build target changed. Rebuilding asset bundles...");

            Build(EditorUserBuildSettings.activeBuildTarget, BuildAssetBundleOptions.StrictMode);
        }

        /*
         * Static.
         */

        public static void Build(BuildTarget target, BuildAssetBundleOptions options)
        {
            Logger.Debug($"Building asset bundles for: {target}");

            if (!Directory.Exists(AssetBundlesDirectory))
                Directory.CreateDirectory(AssetBundlesDirectory);

            var output = BuildPipeline.BuildAssetBundles(AssetBundlesDirectory, options, target);
            if (output == null) // No asset bundles.
            {
                ClearImpl();
                return;
            }
            
            Logger.Debug("Done.");
        }

        public static void Clear()
        {
            if (!Directory.Exists(AssetBundlesDirectory))
                return;
            Logger.Debug("Cleaning asset bundles...");
            ClearImpl();
            Logger.Debug("Done.");
        }

        private static void ClearImpl()
        {
            Directory.Delete(AssetBundlesDirectory, true);
            File.Delete($"{AssetBundlesDirectory}.meta");
        }
    }
}

#endif