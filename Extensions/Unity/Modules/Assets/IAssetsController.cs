using System;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public interface IAssetsController
    {
        /*
         * Bundles.
         */

        bool IsBundleRegistered(int bundleId);
        bool IsBundleLoaded(int bundleId);

        IAssetsController RegisterBundle(int bundleId, string bundleName, params string[] atlasesNames);
        IAssetsController DisposeBundle(int bundleId, bool unloadObjects);

        void DisposeAllBundles(bool unloadObjects);

        void LoadBundle(int bundleId, Action onComplete);
        void LoadBundle(int bundleId, Action onComplete, Action<AssetsException> onError);
        void LoadBundle(int bundleId, Action<AssetBundle> onComplete);
        void LoadBundle(int bundleId, Action<AssetBundle> onComplete, Action<AssetsException> onError);

        void UnloadBundle(int bundleId, bool unloadObjects);
        void UnloadAllBundles(bool unloadObjects);

        /*
         * Assets.
         */

        T    GetAsset<T>(int bundleId, string assetName) where T : Object;
        bool TryGetAsset<T>(int bundleId, string assetName, out T asset) where T : Object;
    }
}