#if UNITY_EDITOR

using UnityEditor;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Editor
{
    public static class BuildNumberMenu
    {
        [MenuItem("PostMVC/Build Number/Increment", false, 10)]
        public static void IncrementBuildNumber()
        {
            BuildNumberProcessor.IncrementBuildNumber();
        }

        [MenuItem("PostMVC/Build Number/Update", false, 11)]
        public static void UpdateBuildNumber()
        {
            BuildNumberProcessor.UpdateBuildNumberFromProjectSettings();
        }

        [MenuItem("PostMVC/Build Number/Reset", false, 50)]
        public static void ResetBuildNumber()
        {
            BuildNumberProcessor.SetBuildNumber(0);
        }
    }
}

#endif