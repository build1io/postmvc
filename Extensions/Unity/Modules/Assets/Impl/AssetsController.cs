using System;
using System.Collections.Generic;
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
        [Inject]                public IAgentsController AgentsController { get; set; }

        public AssetsAtlasProcessingMode AtlasProcessingMode = AssetsAtlasProcessingMode.Strict;

        private readonly Dictionary<Enum, AssetBundle>   _registered;
        private readonly List<AssetBundle>               _bundlesLoaded;
        private readonly Dictionary<string, AssetBundle> _bundleByAtlasId;

        private AssetsAgentBase _assetsAgentBase;

        public AssetsController()
        {
            _registered = new Dictionary<Enum, AssetBundle>();
            _bundlesLoaded = new List<AssetBundle>();
            _bundleByAtlasId = new Dictionary<string, AssetBundle>();
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

        public void RegisterBundle(Enum bundleId, string name, params string[] atlasesNames)
        {
            if (_registered.ContainsKey(bundleId))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, bundleId.ToString());
            _registered.Add(bundleId, new AssetBundle(bundleId, name, atlasesNames));
        }
        
        public void RegisterBundle(AssetBundle bundle)
        {
            if (_registered.ContainsKey(bundle.id))
                throw new AssetsException(AssetsExceptionType.BundleAlreadyRegistered, bundle.id.ToString());
            _registered.Add(bundle.id, bundle);
        }

        /*
         * Loading.
         */
        
        public bool IsBundleLoaded(Enum bundleId)
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            return bundle.IsLoaded;
        }
        
        public bool IsBundleLoaded(AssetBundle bundle)
        {
            return bundle.IsLoaded;
        }
        
        public void LoadBundle(Enum bundleId, Action<AssetBundle> onComplete, Action<AssetsException> onError)
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            LoadBundle(bundle, onComplete, onError);
        }
        
        public void LoadBundle(AssetBundle bundle, Action<AssetBundle> onComplete, Action<AssetsException> onError)
        {
            if (bundle.IsLoaded)
            {
                Log.Debug(n => $"Bundle already loaded: {n}", bundle.name);
                onComplete?.Invoke(bundle);
                return;
            }

            _assetsAgentBase.LoadAssetBundleAsync(bundle.name, unityBundle =>
            {
                Log.Debug(n => $"Bundle loaded: {n}", bundle.name);
                SetBundleLoaded(bundle, unityBundle);
                onComplete?.Invoke(bundle);
            }, exception =>
            {
                Log.Error(exception);
                onError?.Invoke(exception);
            });
        }
        
        private void SetBundleLoaded(AssetBundle bundle, UnityEngine.AssetBundle unityBundle)
        {
            bundle.SetBundle(unityBundle);

            _bundlesLoaded.Add(bundle);

            if (bundle.atlasesNames.Length <= 0)
                return;

            foreach (var atlasesName in bundle.atlasesNames)
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

        public void UnloadBundle(AssetBundle bundle, bool unloadObjects)
        {
            if (!bundle.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundle.name);

            bundle.Bundle.Unload(unloadObjects);
            SetBundleUnloaded(bundle);

            Log.Debug(n => $"Bundle unloaded: {n}", bundle.name);
        }

        public void UnloadAllBundles(bool unloadObjects)
        {
            foreach (var bundle in _bundlesLoaded)
                UnloadBundle(bundle, unloadObjects);
        }

        private void SetBundleUnloaded(AssetBundle bundle)
        {
            bundle.SetBundle(null);

            _bundlesLoaded.Remove(bundle);

            if (bundle.atlasesNames.Length <= 0)
                return;

            foreach (var atlasesName in bundle.atlasesNames)
                _bundleByAtlasId.Remove(atlasesName);
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

        public T GetAsset<T>(AssetBundle bundle, string assetName) where T : UnityEngine.Object
        {
            if (!bundle.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundle.name);
            var asset = (T)bundle.Bundle.LoadAsset(assetName, typeof(T));
            if (asset == null)
                throw new AssetsException(AssetsExceptionType.AssetNotFound, $"Asset: \"{assetName}\" Bundle: \"{bundle.name}\"");
            return asset;
        }

        public bool TryGetAsset<T>(Enum bundleId, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!_registered.TryGetValue(bundleId, out var bundle))
                throw new AssetsException(AssetsExceptionType.BundleNotRegistered, bundleId.ToString());
            return TryGetAsset(bundle, assetName, out asset);
        }
        
        public bool TryGetAsset<T>(AssetBundle bundle, string assetName, out T asset) where T : UnityEngine.Object
        {
            if (!bundle.IsLoaded)
                throw new AssetsException(AssetsExceptionType.BundleNotLoaded, bundle.name);
            asset = (T)bundle.Bundle.LoadAsset(assetName, typeof(T));
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