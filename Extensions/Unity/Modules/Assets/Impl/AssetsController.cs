using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Cache;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using UnityEngine.U2D;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl
{
    internal sealed class AssetsController : IAssetsController
    {
        [Log(LogLevel.Warning)] public ILog              Log              { get; set; }
        [Inject]                public IEventDispatcher  Dispatcher       { get; set; }
        [Inject]                public IAgentsController AgentsController { get; set; }
        [Inject]                public IInjectionBinder  InjectionBinder  { get; set; }

        public AssetsAtlasProcessingMode AtlasProcessingMode = AssetsAtlasProcessingMode.Strict;

        public AssetBundleInfoSetting BundleInfoSettings = AssetBundleInfoSetting.RequestMissingInfo |
                                                           AssetBundleInfoSetting.CacheInfo |
                                                           AssetBundleInfoSetting.CreateMissingInfo;

        public event Func<string, AssetBundleInfo> OnBundleInfoRequest;

        private readonly Dictionary<Enum, string>            _ids;
        private readonly Dictionary<string, AssetBundleInfo> _bundles;
        private readonly Dictionary<string, AssetBundleInfo> _bundlesInfoCache;
        private readonly List<AssetBundleInfo>               _bundlesLoaded;
        private readonly Dictionary<string, AssetBundleInfo> _bundleByAtlasId;

        private AssetsAgentBase             _agent;
        private AssetBundlesCacheController _cacheController;

        public AssetsController()
        {
            _ids = new Dictionary<Enum, string>();
            _bundles = new Dictionary<string, AssetBundleInfo>();
            _bundlesInfoCache = new Dictionary<string, AssetBundleInfo>();
            _bundlesLoaded = new List<AssetBundleInfo>();
            _bundleByAtlasId = new Dictionary<string, AssetBundleInfo>();
        }

        [PostConstruct]
        public void PostConstruct()
        {
            InitializeAgent();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            DisposeAgent();
            DisposeCache();
        }

        /*
         * Agent.
         */

        private void InitializeAgent()
        {
            if (_agent != null)
                throw new AssetsException(AssetsExceptionType.AgentAlreadyInitialised);

            #if UNITY_WEBGL && !UNITY_EDITOR
            _agent = AgentsController.Create<AssetsAgentWebGL>();
            #else
            _agent = AgentsController.Create<AssetsAgentDefault>();
            #endif

            _agent.AtlasRequested += OnAtlasRequested;
        }

        private void DisposeAgent()
        {
            if (_agent == null)
                return;

            _agent.AtlasRequested -= OnAtlasRequested;
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Check.
         */

        public bool CheckBundleLoaded(Enum identifier)
        {
            return CheckBundleLoaded(GetBundleStringId(identifier));
        }

        public bool CheckBundleLoaded(string identifier)
        {
            return _bundles.TryGetValue(identifier, out var info) && info.IsLoaded;
        }

        public bool CheckBundleLoaded(AssetBundleInfo info)
        {
            return _bundles.TryGetValue(info.BundleId, out var infoInner) && infoInner.IsLoaded;
        }

        /*
         * Embed.
         */

        public void LoadEmbedBundle(Enum identifier)   { LoadEmbedBundle(GetBundleStringId(identifier), null, null); }
        public void LoadEmbedBundle(string identifier) { LoadBundle(GetBundleInfoById(identifier), null, null); }

        public void LoadEmbedBundle(Enum identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadEmbedBundle(GetBundleStringId(identifier), onComplete, onError);
        }

        public void LoadEmbedBundle(string identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(GetBundleInfoById(identifier), onComplete, onError);
        }

        /*
         * Remote.
         */

        public void LoadRemoteBundle(string url)
        {
            LoadBundle(GetBundleInfoByUrl(url), null, null);
        }

        public void LoadRemoteBundle(string url, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(GetBundleInfoByUrl(url), onComplete, onError);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version)
        {
            LoadBundle(GetBundleInfoByUrlCached(url, version, null), null, null);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version, string cacheId)
        {
            LoadBundle(GetBundleInfoByUrlCached(url, version, cacheId), null, null);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(GetBundleInfoByUrlCached(url, version, null), onComplete, onError);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version, string cacheId, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(GetBundleInfoByUrlCached(url, version, cacheId), onComplete, onError);
        }

        /*
         * Loading by Info.
         */

        public void LoadBundle(AssetBundleInfo info)
        {
            LoadBundle(info, null, null);
        }

        public void LoadBundle(AssetBundleInfo info, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            Log.Debug(i => $"Load bundle: {i}", info);

            if (_bundles.TryGetValue(info.BundleId, out var infoAdded))
            {
                if (info != infoAdded)
                {
                    Log.Warn(i => $"Bundle with the same id already added. Original bundle used. BundleId: {i}", info.BundleId);

                    info = infoAdded;
                }
            }
            else
            {
                Log.Debug(i => $"Bundle info registered. BundleId: {i}", info.BundleId);

                _bundles.Add(info.BundleId, info);
            }

            if (info.IsLoaded)
            {
                Log.Debug(n => $"Bundle already loaded: {n}", info.ToString());
                onComplete?.Invoke(info);
                return;
            }

            _agent.LoadAsync(info,
                             (bundleInfo) =>
                             {
                                 TryInitializeCache();
                                 return _cacheController.GetBundleCacheInfo(bundleInfo.CacheId);
                             },
                             (bundleInfo) => { _cacheController.CleanBundleCacheInfo(bundleInfo.CacheId); },
                             (bundleName, bundleInfo) => { _cacheController.RecordCacheInfo(bundleInfo.CacheId, bundleName, bundleInfo.BundleUrl, bundleInfo.BundleVersion, bundleInfo.DownloadedBytes); },
                             (bundleInfo, progress, downloadedBytes) =>
                             {
                                 Log.Debug((p, n) => $"Bundle progress: {p} {n}", progress, bundleInfo.ToString());

                                 bundleInfo.SetLoadingProgress(progress, downloadedBytes);

                                 Dispatcher.Dispatch(AssetsEvent.BundleLoadingProgress, bundleInfo);
                             },
                             (bundleInfo, unityBundle) =>
                             {
                                 Log.Debug(n => $"Bundle loaded: {n}", bundleInfo.BundleId);

                                 SetBundleLoaded(bundleInfo, unityBundle);

                                 onComplete?.Invoke(bundleInfo);
                                 Dispatcher.Dispatch(AssetsEvent.BundleLoadingSuccess, bundleInfo);
                             },
                             (bundleInfo, exception) =>
                             {
                                 Log.Error(exception);

                                 SetBundleUnloaded(bundleInfo);

                                 onError?.Invoke(exception);
                                 Dispatcher.Dispatch(AssetsEvent.BundleLoadingFail, bundleInfo, exception);
                             });

            info.SetLoading();
        }

        /*
         * Loading Aborting.
         */

        public void AbortBundleLoading(Enum identifier)
        {
            AbortBundleLoading(GetBundleStringId(identifier));
        }

        public void AbortBundleLoading(string identifier)
        {
            if (!TryGetBundleInfo(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle);

            AbortBundleLoading(info);
        }

        public void AbortBundleLoading(AssetBundleInfo info)
        {
            if (!_bundles.ContainsValue(info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle);

            if (info.IsLoaded)
            {
                Log.Warn(n => $"Bundle loaded: {n}", info.ToString());
                return;
            }

            info.SetAborted();
        }

        /*
         * Unloading.
         */

        public void UnloadBundle(Enum identifier, bool unloadObjects)
        {
            UnloadBundle(GetBundleStringId(identifier), unloadObjects);
        }

        public void UnloadBundle(string identifier, bool unloadObjects)
        {
            if (!TryGetBundleInfo(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle, identifier);
            UnloadBundle(info, unloadObjects);
        }

        public void UnloadBundle(AssetBundleInfo bundleInfo, bool unloadObjects)
        {
            if (!bundleInfo.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundleInfo.BundleId);

            bundleInfo.Bundle.Unload(unloadObjects);
            SetBundleUnloaded(bundleInfo);

            Log.Debug(n => $"Bundle unloaded: {n}", bundleInfo.BundleId);
        }

        public void UnloadAllBundles(bool unloadObjects)
        {
            foreach (var bundle in _bundlesLoaded)
                UnloadBundle(bundle, unloadObjects);
        }

        /*
         * Getting Bundle.
         */

        public AssetBundleInfo GetBundle(Enum identifier)
        {
            return GetBundle(GetBundleStringId(identifier));
        }

        public AssetBundleInfo GetBundle(string identifier)
        {
            if (!TryGetBundleInfo(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle, identifier);

            if (!info.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.BundleId);

            return info;
        }

        /*
         * Assets.
         */

        public T GetAsset<T>(Enum identifier, string assetName) where T : UnityEngine.Object
        {
            return GetAsset<T>(GetBundleStringId(identifier), assetName);
        }

        public T GetAsset<T>(string identifier, string assetName) where T : UnityEngine.Object
        {
            if (!TryGetBundleInfo(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle, identifier);

            return GetAsset<T>(info, assetName);
        }

        public T GetAsset<T>(AssetBundleInfo info, string assetName) where T : UnityEngine.Object
        {
            if (!info.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.BundleId);

            var asset = (T)info.Bundle.LoadAsset(assetName, typeof(T));
            if (asset == null)
                throw new AssetsException(AssetsExceptionType.AssetNotFound, $"Asset: \"{assetName}\" Bundle: \"{info.BundleId}\"");

            return asset;
        }

        public bool TryGetAsset<T>(Enum identifier, string assetName, out T asset) where T : UnityEngine.Object
        {
            return TryGetAsset(GetBundleStringId(identifier), assetName, out asset);
        }

        public bool TryGetAsset<T>(string identifier, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!TryGetBundleInfo(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle, identifier);

            return TryGetAsset(info, assetName, out asset);
        }

        public bool TryGetAsset<T>(AssetBundleInfo info, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!info.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.BundleId);

            asset = (T)info.Bundle.LoadAsset(assetName, typeof(T));
            return asset != null;
        }

        /*
         * Cache.
         */

        public ulong GetBundleCacheSizeByCacheId(string cacheId)
        {
            TryInitializeCache();
            return _cacheController.GetBundleCacheInfo(cacheId)?.BundleSizeBytes ?? 0;
        }

        public ulong GetCachedFilesSizeBytes()
        {
            ulong size = 0;
            var paths = new List<string>();

            Caching.GetAllCachePaths(paths);

            foreach (var path in paths)
            {
                var cache = Caching.GetCacheByPath(path);
                size += (ulong)cache.spaceOccupied;
            }

            return size;
        }

        public void CleanCache()
        {
            Caching.ClearCache();
        }

        private void TryInitializeCache()
        {
            _cacheController ??= InjectionBinder.Construct<AssetBundlesCacheController>(true);
        }

        private void DisposeCache()
        {
            if (_cacheController == null)
                return;

            _cacheController.Destroy(InjectionBinder, true);
            _cacheController = null;
        }

        /*
         * Bundle Info.
         */

        private AssetBundleInfo GetBundleInfoById(string identifier)
        {
            if (TryGetBundleInfo(identifier, out var info))
                return info;

            if ((BundleInfoSettings & AssetBundleInfoSetting.CreateMissingInfo) == AssetBundleInfoSetting.CreateMissingInfo)
                info = AssetBundleInfo.FromId(identifier);

            return info;
        }

        private AssetBundleInfo GetBundleInfoByUrl(string url)
        {
            if (TryGetBundleInfo(url, out var info))
                return info;

            if ((BundleInfoSettings & AssetBundleInfoSetting.CreateMissingInfo) == AssetBundleInfoSetting.CreateMissingInfo)
                info = AssetBundleInfo.FromUrl(url);

            return info;
        }

        private AssetBundleInfo GetBundleInfoByUrlCached(string url, uint version, string cacheId)
        {
            if (TryGetBundleInfo(url, out var info))
                return info;

            if ((BundleInfoSettings & AssetBundleInfoSetting.CreateMissingInfo) == AssetBundleInfoSetting.CreateMissingInfo)
                info = AssetBundleInfo.FromUrlCached(url, version, cacheId);

            return info;
        }

        private bool TryGetBundleInfo(string identifier, out AssetBundleInfo info)
        {
            if (_bundles.TryGetValue(identifier, out info))
            {
                Log.Debug(i => $"Bundle info found. {i}", info);
                return true;
            }

            if (_bundlesInfoCache.TryGetValue(identifier, out info))
            {
                Log.Debug(i => $"Bundle info found in info cache. {i}", info);
                return true;
            }

            if ((BundleInfoSettings & AssetBundleInfoSetting.RequestMissingInfo) == AssetBundleInfoSetting.RequestMissingInfo && OnBundleInfoRequest != null)
            {
                Log.Debug(i => $"Requesting bundle info: {i}", identifier);

                info = OnBundleInfoRequest.Invoke(identifier);

                if (info != null && (BundleInfoSettings & AssetBundleInfoSetting.CacheInfo) == AssetBundleInfoSetting.CacheInfo)
                {
                    Log.Debug(i => $"Bundle info saved to cache. {i}", info);

                    _bundlesInfoCache.Add(identifier, info);
                }
            }

            return info != null;
        }

        /*
         * Helpers.
         */

        private string GetBundleStringId(Enum identifier)
        {
            if (_ids.TryGetValue(identifier, out var id))
                return id;

            id = AssetBundleInfo.EnumToStringIdentifier(identifier);
            _ids.Add(identifier, id);

            return id;
        }

        private void SetBundleLoaded(AssetBundleInfo info, AssetBundle unityBundle)
        {
            info.SetLoaded(unityBundle);

            _bundlesLoaded.Add(info);

            if (!info.HasAtlases)
                return;

            foreach (var atlasesName in info.AtlasesNames)
                _bundleByAtlasId.Add(atlasesName, info);
        }

        private void SetBundleUnloaded(AssetBundleInfo info)
        {
            info.Clean();

            // _bundles.Remove(info.BundleId);
            _bundlesLoaded.Remove(info);

            if (!info.HasAtlases)
                return;

            foreach (var atlasesName in info.AtlasesNames)
                _bundleByAtlasId.Remove(atlasesName);
        }

        /*
         * Event handlers.
         */

        private SpriteAtlas OnAtlasRequested(string atlasId)
        {
            _bundleByAtlasId.TryGetValue(atlasId, out var bundle);

            if (bundle == null || !bundle.IsLoaded)
            {
                if (AtlasProcessingMode == AssetsAtlasProcessingMode.Strict)
                    throw new AssetsException(AssetsExceptionType.AtlasBundleNotLoaded, atlasId);
                return null;
            }

            var atlas = GetAsset<SpriteAtlas>(bundle, atlasId);
            if (atlas != null)
                return atlas;

            if (AtlasProcessingMode == AssetsAtlasProcessingMode.Strict)
                throw new AssetsException(AssetsExceptionType.AtlasNotFound, $"Atlas: \"{atlas}\" Bundle: \"{bundle}\"");

            return null;
        }
    }
}