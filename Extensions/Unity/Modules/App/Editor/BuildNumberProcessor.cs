#if UNITY_EDITOR

using System;
using Build1.PostMVC.Extensions.Unity.Modules.App.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Editor
{
    public abstract class BuildNumberProcessor
    {
        private static readonly ILogger Logger = LoggerProvider.GetLogger<BuildNumberProcessor>(LogLevel.All);

        private const string BuildNumberFileFullName = AppController.BuildNumberFileName + ".txt";
        private const string BuildNumberFolderPath   = "/Resources";
        private const string BuildNumberFilePath     = BuildNumberFolderPath + "/" + BuildNumberFileFullName;
        
        public static void SetBuildNumber(int buildNumber)
        {
            SetBuildNumber(buildNumber.ToString());
        }

        public static void IncrementBuildNumber()
        {
            if (CheckBuildNumberIncrementApplicability())
                SetBuildNumber(AppController.GetBuildNumber() + 1);
        }

        public static void UpdateBuildNumberFromProjectSettings()
        {
            var buildNumber = GetBuildNumberFromProjectSettings();
            if (buildNumber != null)
                SetBuildNumber(buildNumber);
        }

        public static void SetBuildNumber(string buildNumber)
        {
            var path = Application.dataPath + BuildNumberFilePath;
            if (System.IO.File.Exists(path))
            {
                var buildNumberCurrent = System.IO.File.ReadAllText(path);
                if (buildNumber == buildNumberCurrent)
                    return;
            }

            var folderPath = Application.dataPath + BuildNumberFolderPath;
            if (!System.IO.Directory.Exists(folderPath))
                System.IO.Directory.CreateDirectory(folderPath);

            System.IO.File.WriteAllText(path, buildNumber);

            Logger.Debug($"Build number file updated. BuildNumber: {buildNumber}");
        }

        private static string GetBuildNumberFromProjectSettings()
        {
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case UnityEditor.BuildTarget.iOS:     return UnityEditor.PlayerSettings.iOS.buildNumber;
                case UnityEditor.BuildTarget.Android: return UnityEditor.PlayerSettings.Android.bundleVersionCode.ToString();
                case UnityEditor.BuildTarget.WebGL:   return null; // Not applicable for WebGL.

                default:
                    throw new Exception($"Not implemented for build target: {target}");
            }
        }

        private static bool CheckBuildNumberIncrementApplicability()
        {
            var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
            switch (target)
            {
                case UnityEditor.BuildTarget.WebGL:
                    return true;

                case UnityEditor.BuildTarget.iOS:
                case UnityEditor.BuildTarget.Android:
                    Logger.Warn($"Not applicable for: {target}");
                    return false;

                default:
                    throw new Exception($"Not implemented for build target: {target}");
            }
        }
    }
}

#endif