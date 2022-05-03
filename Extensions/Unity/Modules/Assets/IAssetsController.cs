using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public interface IAssetsController
    {
        event Func<string, AssetBundleInfo> OnBundleInfoRequest;

        /*
         * Check.
         */

        bool CheckBundleLoaded(Enum identifier);
        bool CheckBundleLoaded(string identifier);
        bool CheckBundleLoaded(AssetBundleInfo info);

        /*
         * Embed.
         */

        void LoadEmbedBundle(Enum identifier);
        void LoadEmbedBundle(string identifier);

        void LoadEmbedBundle(Enum identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);
        void LoadEmbedBundle(string identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        /*
         * Remote.
         */

        void LoadRemoteBundle(string url);
        void LoadRemoteBundle(string url, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        void LoadRemoteOrCachedBundle(string url, uint version);
        void LoadRemoteOrCachedBundle(string url, uint version, string cacheId);
        void LoadRemoteOrCachedBundle(string url, uint version, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);
        void LoadRemoteOrCachedBundle(string url, uint version, string cacheId, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        /*
         * Loading by Info.
         */

        void LoadBundle(AssetBundleInfo info);
        void LoadBundle(AssetBundleInfo info, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError);

        /*
         * Loading Aborting.
         */

        void AbortBundleLoading(Enum identifier);
        void AbortBundleLoading(string identifier);
        void AbortBundleLoading(AssetBundleInfo info);

        /*
         * Unloading.
         */

        void UnloadBundle(Enum identifier, bool unloadObjects);
        void UnloadBundle(string identifier, bool unloadObjects);
        void UnloadBundle(AssetBundleInfo bundleInfo, bool unloadObjects);

        void UnloadAllBundles(bool unloadObjects);

        /*
         * Getting.
         */

        AssetBundleInfo GetBundle(Enum identifier);
        AssetBundleInfo GetBundle(string identifier);

        /*
         * Cache.
         */

        ulong GetBundleCacheSizeByCacheId(string cacheId);
        ulong GetCachedFilesSizeBytes();
        void  CleanCache();

        /*
         * Assets.
         */

        T GetAsset<T>(Enum identifier, string assetName) where T : UnityEngine.Object;
        T GetAsset<T>(string identifier, string assetName) where T : UnityEngine.Object;
        T GetAsset<T>(AssetBundleInfo bundleInfo, string assetName) where T : UnityEngine.Object;

        bool TryGetAsset<T>(Enum identifier, string assetName, out T asset) where T : UnityEngine.Object;
        bool TryGetAsset<T>(string identifier, string assetName, out T asset) where T : UnityEngine.Object;
        bool TryGetAsset<T>(AssetBundleInfo bundleInfo, string assetName, out T asset) where T : UnityEngine.Object;
    }
}