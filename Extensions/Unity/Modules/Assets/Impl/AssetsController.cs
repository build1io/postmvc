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

        private readonly Dictionary<Enum, AssetBundleInfo>   _registered;
        private readonly List<AssetBundleInfo>               _bundlesLoaded;
        private readonly Dictionary<string, AssetBundleInfo> _bundleByAtlasId;

        private AssetsAgentBase _assetsAgentBase;

        public AssetsController()
        {
            _registered = new Dictionary<Enum, AssetBundleInfo>();
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
            if (_assetsAgentBase != null)
                throw new Exception("AssetsController already initialized.");

            #if UNITY_WEBGL && !UNITY_EDITOR
            _assetsAgentBase = AgentsController.Create<AssetsAgentWebGL>();
            #else
            _assetsAgentBase = AgentsController.Create<AssetsAgentDefault>();
            #endif

            _assetsAgentBase.AtlasRequested += OnAtlasRequested;
        }

        private void DisposeAgent()
        {
            if (_assetsAgentBase == null)
                return;

            _assetsAgentBase.AtlasRequested -= OnAtlasRequested;
            AgentsController.Destroy(ref _assetsAgentBase);
        }

        /*
         * Registration.
         */

        public AssetBundleInfo RegisterBundle(Enum bundleId, string name, params string[] atlasesNames)
        {
            if (_registered.ContainsKey(bundleId))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, bundleId.ToString());

            var info = new AssetBundleInfo(bundleId, name, atlasesNames);
            _registered.Add(bundleId, info);

            return info;
        }

        public AssetBundleInfo RegisterBundle(Enum bundleId, string name, string url, params string[] atlasesNames)
        {
            if (_registered.ContainsKey(bundleId))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, bundleId.ToString());

            var info = new AssetBundleInfo(bundleId, name, url, atlasesNames);
            _registered.Add(bundleId, info);

            return info;
        }

        public void RegisterBundle(AssetBundleInfo info)
        {
            if (_registered.ContainsKey(info.bundleId))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, info.bundleId.ToString());
            _registered.Add(info.bundleId, info);
        }

        /*
         * Loading.
         */

        public bool IsBundleLoaded(Enum bundleId)
        {
            if (!_registered.TryGetValue(bundleId, out var info))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            return info.IsLoaded;
        }

        public bool IsBundleLoaded(AssetBundleInfo bundleInfo)
        {
            return bundleInfo.IsLoaded;
        }

        public void LoadBundle(Enum bundleId)
        {
            LoadBundle(bundleId, null, null);
        }

        public void LoadBundle(Enum bundleId, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            LoadBundle(bundle, onComplete, onError);
        }
        
        public void LoadBundle(AssetBundleInfo bundleInfo)
        {
            LoadBundle(bundleInfo, null, null);
        }

        public void LoadBundle(AssetBundleInfo bundleInfo, Action<AssetBundleInfo> onComplete, Action<AssetsException> onError)
        {
            if (bundleInfo.IsLoaded)
            {
                Log.Debug(n => $"Bundle already loaded: {n}", bundleInfo.bundleName);
                onComplete?.Invoke(bundleInfo);
                return;
            }

            _assetsAgentBase.LoadAssetBundleAsync(bundleInfo, 
                                                  progress =>
                                                  {
                                                      Log.Debug((p, n) => $"Bundle progress: {p} {n}", progress, bundleInfo.bundleName);
                                                      
                                                      Dispatcher.Dispatch(AssetsEvent.BundleLoadingProgress, bundleInfo.bundleId, progress);
                                                  },
                                                  unityBundle =>
                                                  {
                                                      Log.Debug(n => $"Bundle loaded: {n}", bundleInfo.bundleName);

                                                      SetBundleLoaded(bundleInfo, unityBundle);

                                                      onComplete?.Invoke(bundleInfo);
                                                      Dispatcher.Dispatch(AssetsEvent.BundleLoadingSuccess, bundleInfo);
                                                  }, exception =>
                                                  {
                                                      Log.Error(exception);

                                                      onError?.Invoke(exception);
                                                      Dispatcher.Dispatch(AssetsEvent.BundleLoadingFail, exception);
                                                  });
        }

        private void SetBundleLoaded(AssetBundleInfo bundle, UnityEngine.AssetBundle unityBundle)
        {
            bundle.SetBundle(unityBundle);

            _bundlesLoaded.Add(bundle);

            if (!bundle.HasAtlases)
                return;
            
            foreach (var atlasesName in bundle.AtlasesNames)
                _bundleByAtlasId.Add(atlasesName, bundle);
        }

        /*
         * Unloading.
         */

        public void UnloadBundle(Enum bundleId, bool unloadObjects)
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            UnloadBundle(bundle, unloadObjects);
        }

        public void UnloadBundle(AssetBundleInfo bundleInfo, bool unloadObjects)
        {
            if (!bundleInfo.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundleInfo.bundleName);

            bundleInfo.Bundle.Unload(unloadObjects);
            SetBundleUnloaded(bundleInfo);

            Log.Debug(n => $"Bundle unloaded: {n}", bundleInfo.bundleName);
        }

        public void UnloadAllBundles(bool unloadObjects)
        {
            foreach (var bundle in _bundlesLoaded)
                UnloadBundle(bundle, unloadObjects);
        }

        private void SetBundleUnloaded(AssetBundleInfo bundle)
        {
            bundle.SetBundle(null);

            _bundlesLoaded.Remove(bundle);

            if (!bundle.HasAtlases)
                return;

            foreach (var atlasesName in bundle.AtlasesNames)
                _bundleByAtlasId.Remove(atlasesName);
        }

        /*
         * Getting.
         */

        public AssetBundleInfo GetBundle(Enum bundleId)
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());

            if (!bundle.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundle.bundleName);

            return bundle;
        }

        /*
         * Assets.
         */

        public T GetAsset<T>(Enum bundleId, string assetName) where T : UnityEngine.Object
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            return GetAsset<T>(bundle, assetName);
        }

        public T GetAsset<T>(AssetBundleInfo bundleInfo, string assetName) where T : UnityEngine.Object
        {
            if (!bundleInfo.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundleInfo.bundleName);
            var asset = (T)bundleInfo.Bundle.LoadAsset(assetName, typeof(T));
            if (asset == null)
                throw new AssetsException(AssetsExceptionType.AssetNotFound, $"Asset: \"{assetName}\" Bundle: \"{bundleInfo.bundleName}\"");
            return asset;
        }

        public bool TryGetAsset<T>(Enum bundleId, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            return TryGetAsset(bundle, assetName, out asset);
        }

        public bool TryGetAsset<T>(AssetBundleInfo bundleInfo, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!bundleInfo.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundleInfo.bundleName);
            asset = (T)bundleInfo.Bundle.LoadAsset(assetName, typeof(T));
            return asset != null;
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