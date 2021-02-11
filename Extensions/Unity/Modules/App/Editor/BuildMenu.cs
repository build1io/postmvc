#if UNITY_EDITOR

using UnityEditor;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Editor
{
    public static class BuildMenu
    {
        [MenuItem("Build/Increment Build Number", false, 10)]
        public static void IncrementBuildNumber()
        {
            BuildNumberProcessor.IncrementBuildNumber();
        }

        [MenuItem("Build/Update Build Number", false, 30)]
        public static void UpdateBuildNumber()
        {
            BuildNumberProcessor.UpdateBuildNumberFromProjectSettings();
        }

        [MenuItem("Build/Reset Build Number", false, 50)]
        public static void ResetBuildNumber()
        {
            BuildNumberProcessor.SetBuildNumber(0);
        }
    }
}

#endif