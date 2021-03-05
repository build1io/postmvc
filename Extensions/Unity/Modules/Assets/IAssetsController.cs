using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public interface IAssetsController
    {
        /*
         * Bundles.
         */

        bool IsBundleLoaded(AssetBundle bundle);
        
        void LoadBundle(AssetBundle bundle, Action<AssetBundle> onComplete, Action<AssetsException> onError);
        void UnloadBundle(AssetBundle bundle, bool unloadObjects);
        void UnloadAllBundles(bool unloadObjects);

        /*
         * Assets.
         */

        T    GetAsset<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object;
        bool TryGetAsset<T>(AssetBundle bundle, string assetName, out T asset) where T : UnityEngine.Object;
    }
}