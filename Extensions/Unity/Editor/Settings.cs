#if UNITY_EDITOR

using UnityEditor;

namespace Build1.PostMVC.Extensions.Unity.Editor
{
    public static class Settings
    {
        private const string RefreshAssetsOnPlayMenuItemName = "PostMVC/Settings/Refresh Assets on Play Mode";

        [MenuItem(RefreshAssetsOnPlayMenuItemName, false, 10)]
        public static void RefreshAssetsOnPlay()
        {
            var enabled = AssetsRefreshTool.GetEnabled();
            AssetsRefreshTool.SetEnabled(!enabled);
            Menu.SetChecked(RefreshAssetsOnPlayMenuItemName, !enabled);
        }
    }
}

#endif