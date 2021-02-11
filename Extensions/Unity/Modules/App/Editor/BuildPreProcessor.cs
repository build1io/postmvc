#if UNITY_EDITOR

using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace Build1.PostMVC.Extensions.Unity.Modules.App.Editor
{
    public sealed class BuildPreProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;
        
        public void OnPreprocessBuild(BuildReport report)
        {
            BuildNumberProcessor.UpdateBuildNumberFromProjectSettings();
        }
    }
}

#endif