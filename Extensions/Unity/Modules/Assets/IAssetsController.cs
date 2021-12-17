using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public interface IAssetsController
    {
        /*
         * Registration.
         */

        void RegisterBundle(Enum bundleId, string name, params string[] atlasesNames);
        void RegisterBundle(AssetBundle bundle);

        /*
         * Loading.
         */
        
        bool IsBundleLoaded(Enum bundleId);
        bool IsBundleLoaded(AssetBundle bundle);

        void LoadBundle(Enum bundleId, Action<AssetBundle> onComplete, Action<AssetsException> onError);
        void LoadBundle(AssetBundle bundle, Action<AssetBundle> onComplete, Action<AssetsException> onError);

        /*
         * Unloading.
         */
        
        void UnloadBundle(Enum bundleId, bool unloadObjects);
        void UnloadBundle(AssetBundle bundle, bool unloadObjects);

        void UnloadAllBundles(bool unloadObjects);

        /*
         * Getting.
         */

        AssetBundle GetBundle(Enum bundleId);
        
        /*
         * Assets.
         */

        T GetAsset<T>(Enum bundleId, string assetName) where T : UnityEngine.Object;
        T GetAsset<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object;

        bool TryGetAsset<T>(Enum bundleId, string assetName, out T asset) where T : UnityEngine.Object;
        bool TryGetAsset<T>(AssetBundle bundle, string assetName, out T asset) where T : UnityEngine.Object;
    }
}