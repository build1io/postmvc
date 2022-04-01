using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine.U2D;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl
{
    internal sealed class AssetsController : IAssetsController
    {
        [Log(LogLevel.Warning)] public ILog              Log              { get; set; }
        [Inject]                public IEventDispatcher  Dispatcher       { get; set; }
        [Inject]                public IAgentsController AgentsController { get; set; }

        public AssetsAtlasProcessingMode AtlasProcessingMode = AssetsAtlasProcessingMode.Strict;

        private readonly Dictionary<string, AssetBundleInfo> _bundles;
        private readonly List<AssetBundleInfo>               _bundlesLoaded;
        private readonly Dictionary<string, AssetBundleInfo> _bundleByAtlasId;

        private AssetsAgentBase _agent;

        public AssetsController()
        {
            _bundles = new Dictionary<string, AssetBundleInfo>();
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
        }

        /*
         * Agent.
         */

        private void InitializeAgent()
        {
            if (_agent != null)
                throw new Exception("AssetsController already initialized.");

            #if UNITY_WEBGL && !UNITY_EDITOR
            _assetsAgentBase = AgentsController.Create<AssetsAgentWebGL>();
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
            return CheckBundleLoaded(AssetBundleInfo.EnumToStringIdentifier(identifier));
        }

        public bool CheckBundleLoaded(string identifier)
        {
            return _bundles.TryGetValue(identifier, out var info) && info.IsLoaded;
        }

        public bool CheckBundleLoaded(AssetBundleInfo info)
        {
            return info.IsLoaded;
        }

        /*
         * Embed.
         */

        public void LoadEmbedBundle(Enum identifier)   { LoadBundle(AssetBundleInfo.FromId(identifier), null, null); }
        public void LoadEmbedBundle(string identifier) { LoadBundle(AssetBundleInfo.FromId(identifier), null, null); }

        public void LoadEmbedBundle(Enum identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(AssetBundleInfo.FromId(identifier), onComplete, onError);
        }

        public void LoadEmbedBundle(string identifier, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(AssetBundleInfo.FromId(identifier), onComplete, onError);
        }

        /*
         * Remote.
         */

        public void LoadRemoteBundle(string url)
        {
            LoadBundle(AssetBundleInfo.FromUrl(url), null, null);
        }

        public void LoadRemoteBundle(string url, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(AssetBundleInfo.FromUrl(url), onComplete, onError);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version)
        {
            LoadBundle(AssetBundleInfo.FromUrl(url, true, version), null, null);
        }

        public void LoadRemoteOrCachedBundle(string url, uint version, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            LoadBundle(AssetBundleInfo.FromUrl(url, true, version), onComplete, onError);
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
            if (_bundles.TryGetValue(info.BundleId, out var infoAdded))
            {
                Log.Warn(i => $"Bundle with the same id already added. Original bundle used. BundleId: {i}", info.BundleId);

                info = infoAdded;
            }
            else
            {
                Log.Debug(i => $"Bundle added: {i}", info.BundleId);

                _bundles.Add(info.BundleId, info);
            }

            if (info.IsLoaded)
            {
                Log.Debug(n => $"Bundle already loaded: {n}", info.ToString());
                onComplete?.Invoke(info);
                return;
            }

            _agent.LoadAsync(info,
                             (bundleInfo, progress, downloadedBytes) =>
                             {
                                 Log.Debug((p, n) => $"Bundle progress: {p} {n}", progress, bundleInfo.ToString());

                                 info.SetLoadingProgress(progress, downloadedBytes);

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
            AbortBundleLoading(AssetBundleInfo.EnumToStringIdentifier(identifier));
        }

        public void AbortBundleLoading(string identifier)
        {
            if (!_bundles.TryGetValue(identifier, out var info))
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
            UnloadBundle(AssetBundleInfo.EnumToStringIdentifier(identifier), unloadObjects);
        }

        public void UnloadBundle(string identifier, bool unloadObjects)
        {
            if (!_bundles.TryGetValue(identifier, out var info))
                throw new AssetsException(AssetsExceptionType.UnknownBundle, identifier);
            UnloadBundle(info, unloadObjects);
        }

        public void UnloadBundle(AssetBundleInfo bundleInfo, bool unloadObjects)
        {
            if (!bundleInfo.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundleInfo.BundleId);

            bundleInfo.Bundle.Unload(unloadObjects);
            SetBundleUnloaded(bundleInfo);

            _bundles.Remove(bundleInfo.BundleId);

            Log.Debug(n => $"Bundle unloaded: {n}", bundleInfo.BundleId);
        }

        public void UnloadAllBundles(bool unloadObjects)
        {
            foreach (var bundle in _bundlesLoaded)
                UnloadBundle(bundle, unloadObjects);
        }

        /*
         * Getting.
         */

        public AssetBundleInfo GetBundle(Enum identifier)
        {
            return GetBundle(AssetBundleInfo.EnumToStringIdentifier(identifier));
        }

        public AssetBundleInfo GetBundle(string identifier)
        {
            if (!_bundles.TryGetValue(identifier, out var info))
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
            return GetAsset<T>(AssetBundleInfo.EnumToStringIdentifier(identifier), assetName);
        }

        public T GetAsset<T>(string identifier, string assetName) where T : UnityEngine.Object
        {
            if (!_bundles.TryGetValue(identifier, out var info))
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
            return TryGetAsset(AssetBundleInfo.EnumToStringIdentifier(identifier), assetName, out asset);
        }

        public bool TryGetAsset<T>(string identifier, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!_bundles.TryGetValue(identifier, out var info))
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
         * Helpers.
         */

        private void SetBundleLoaded(AssetBundleInfo info, UnityEngine.AssetBundle unityBundle)
        {
            info.SetLoaded(unityBundle);

            _bundlesLoaded.Add(info);

            if (!info.HasAtlases)
                return;

            foreach (var atlasesName in info.AtlasesNames)
                _bundleByAtlasId.Add(atlasesName, info);
        }

        private void SetBundleUnloaded(AssetBundleInfo bundle)
        {
            bundle.Clean();

            _bundlesLoaded.Remove(bundle);

            if (!bundle.HasAtlases)
                return;

            foreach (var atlasesName in bundle.AtlasesNames)
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