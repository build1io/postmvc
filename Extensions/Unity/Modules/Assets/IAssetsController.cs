using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public interface IAssetsController
    {
        /*
         * Registration.
         */

        AssetBundleInfo RegisterBundle(Enum bundleId, string name, string[] atlasesNames = null);
        AssetBundleInfo RegisterBundle(Enum bundleId, string name, string url, string[] atlasesNames = null);
        void            RegisterBundle(AssetBundleInfo info);

        /*
         * Loading.
         */

        bool IsBundleLoaded(Enum bundleId);
        bool IsBundleLoaded(AssetBundleInfo bundleInfo);

        void LoadBundle(Enum bundleId);
        void LoadBundle(Enum bundleId, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        void LoadBundle(AssetBundleInfo bundleInfo);
        void LoadBundle(AssetBundleInfo bundleInfo, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        /*
         * Unloading.
         */

        void UnloadBundle(Enum bundleId, bool unloadObjects);
        void UnloadBundle(AssetBundleInfo bundleInfo, bool unloadObjects);

        void UnloadAllBundles(bool unloadObjects);

        /*
         * Getting.
         */

        AssetBundleInfo GetBundle(Enum bundleId);

        /*
         * Assets.
         */

        T GetAsset<T>(Enum bundleId, string assetName) where T : UnityEngine.Object;
        T GetAsset<T>(AssetBundleInfo bundleInfo, string assetName) where T : UnityEngine.Object;

        bool TryGetAsset<T>(Enum bundleId, string assetName, out T asset) where T : UnityEngine.Object;
        bool TryGetAsset<T>(AssetBundleInfo bundleInfo, string assetName, out T asset) where T : UnityEngine.Object;
    }
}