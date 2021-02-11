using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents;
using UnityEngine;
using UnityEngine.U2D;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl
{
    internal sealed class AssetsController : IAssetsController
    {
        [Inject] public IAgentsController AgentsController { get; set; }

        public AssetsAtlasProcessingMode AtlasProcessingMode = AssetsAtlasProcessingMode.Strict;
        public int                       BundlesCount        = 0;

        private readonly Dictionary<int, AssetBundleInfo>    _infos;
        private readonly Dictionary<string, AssetBundleInfo> _infosByAtlasId;

        private AssetsAgentBase _assetsAgentBase;

        public AssetsController()
        {
            _infos = new Dictionary<int, AssetBundleInfo>(BundlesCount);
            _infosByAtlasId = new Dictionary<string, AssetBundleInfo>(BundlesCount);
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
         * Bundles.
         */

        public bool IsBundleRegistered(int bundleId)
        {
            return _infos.ContainsKey(bundleId);
        }

        public bool IsBundleLoaded(int bundleId)
        {
            return GetBundleInfo(bundleId).IsLoaded;
        }

        public IAssetsController RegisterBundle(int bundleId, string bundleName, params string[] atlasesNames)
        {
            if (_infos.TryGetValue(bundleId, out var infoTemp))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, infoTemp.ToString());

            var info = new AssetBundleInfo(bundleId, bundleName, atlasesNames);
            _infos.Add(bundleId, info);

            foreach (var atlasId in info.atlasesIds)
                _infosByAtlasId.Add(atlasId, info);

            return this;
        }

        public IAssetsController DisposeBundle(int bundleId, bool unloadObjects)
        {
            var info = GetBundleInfo(bundleId);
            UnloadBundleImpl(info, unloadObjects);

            foreach (var atlasName in info.atlasesIds)
                _infosByAtlasId.Remove(atlasName);

            return this;
        }

        public void DisposeAllBundles(bool unloadObjects)
        {
            foreach (var info in _infos.Values)
                UnloadBundleImpl(info, unloadObjects);
            _infos.Clear();
            _infosByAtlasId.Clear();
        }

        /*
         * Loading.
         */

        public void LoadBundle(int bundleId, Action onComplete)
        {
            LoadBundle(bundleId, onComplete, null);
        }

        public void LoadBundle(int bundleId, Action onComplete, Action<AssetsException> onError)
        {
            var info = GetBundleInfo(bundleId);
            if (info.IsLoaded)
            {
                onComplete?.Invoke();
                return;
            }

            _assetsAgentBase.LoadAssetBundleAsync(info.bundleName, bundle =>
            {
                info.SetBundle(bundle);
                onComplete?.Invoke();
            }, onError);
        }

        public void LoadBundle(int bundleId, Action<AssetBundle> onComplete)
        {
            LoadBundle(bundleId, onComplete, null);
        }

        public void LoadBundle(int bundleId, Action<AssetBundle> onComplete, Action<AssetsException> onError)
        {
            var info = GetBundleInfo(bundleId);
            if (info.IsLoaded)
            {
                onComplete?.Invoke(info.Bundle);
                return;
            }

            _assetsAgentBase.LoadAssetBundleAsync(info.bundleName, bundle =>
            {
                info.SetBundle(bundle);
                onComplete?.Invoke(info.Bundle);
            }, onError);
        }

        /*
         * Unloading.
         */

        public void UnloadBundle(int bundleId, bool unloadObjects)
        {
            UnloadBundleImpl(GetBundleInfo(bundleId), unloadObjects);
        }

        public void UnloadAllBundles(bool unloadObjects)
        {
            foreach (var info in _infos.Values)
                UnloadBundleImpl(info, unloadObjects);
        }

        private void UnloadBundleImpl(AssetBundleInfo info, bool unloadObjects)
        {
            if (!info.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.bundleName);
            info.Bundle.Unload(unloadObjects);
            info.SetBundle(null);
        }

        /*
         * Items.
         */

        public T GetAsset<T>(int bundleId, string assetName) where T : Object
        {
            var info = GetLoadedBundleInfo(bundleId);
            var asset = (T)info.Bundle.LoadAsset(assetName, typeof(T));
            if (asset == null)
                throw new AssetsException(AssetsExceptionType.AssetNotFound, $"Asset: \"{assetName}\" Bundle: \"{info.bundleName}\"");
            return asset;
        }

        public bool TryGetAsset<T>(int bundleId, string assetName, out T asset) where T : Object
        {
            var info = GetLoadedBundleInfo(bundleId);
            asset = (T)info.Bundle.LoadAsset(assetName, typeof(T));
            return asset != null;
        }

        /*
         * Private.
         */

        private AssetBundleInfo GetBundleInfo(int bundleId)
        {
            if (!_infos.TryGetValue(bundleId, out var info))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, $"BundleId: {bundleId}");
            return info;
        }

        private AssetBundleInfo GetLoadedBundleInfo(int bundleId)
        {
            var info = GetBundleInfo(bundleId);
            if (!info.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.bundleName);
            return info;
        }

        /*
         * Event handlers.
         */

        private SpriteAtlas OnAtlasRequested(string atlasId)
        {
            _infosByAtlasId.TryGetValue(atlasId, out var info);

            if (info == null)
            {
                if (AtlasProcessingMode == AssetsAtlasProcessingMode.Strict)
                    throw new AssetsException(AssetsExceptionType.BundleNotRegistered, $"AtlasId: \"{atlasId}\"");
                return null;
            }

            if (!info.IsLoaded)
            {
                if (AtlasProcessingMode == AssetsAtlasProcessingMode.Strict)
                    throw new AssetsException(AssetsExceptionType.BundleNotLoaded, info.bundleName);
                return null;
            }

            var atlas = GetAsset<SpriteAtlas>(info.bundleId, atlasId);
            if (atlas == null)
            {
                if (AtlasProcessingMode == AssetsAtlasProcessingMode.Strict)
                    throw new AssetsException(AssetsExceptionType.AtlasNotFound, $"Atlas: \"{atlas}\" Bundle: \"{info}\"");
                return null;
            }

            return atlas;
        }
    }
}