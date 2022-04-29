using System;
using System.Collections.Generic;
using System.IO;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.App;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Newtonsoft.Json;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Cache
{
    public sealed class AssetBundlesCacheController
    {
        public const string AssetBundlesCacheInfoFileName = "asset_bundles_cache_info.json";

        [Log(LogLevel.Warning)] public ILog           Log           { get; set; }
        [Inject]            public IAppController AppController { get; set; }

        private Dictionary<string, AssetBundleCacheInfo> _infos;
        private string                                   _infosFilesPath;
        
        // TODO: save cache size
        // TODO: ability to clean cache by user request
        // TODO: check what bundle is in cache

        [PostConstruct]
        public void PostConstruct()
        {
            Log.Debug("PostConstruct");

            _infos = new Dictionary<string, AssetBundleCacheInfo>();
            _infosFilesPath = Path.Combine(AppController.PersistentDataPath, AssetBundlesCacheInfoFileName);

            LoadCacheInfo();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            Log.Debug("PreDestroy");
        }

        /*
         * Public.
         */

        public AssetBundleCacheInfo GetBundleCacheInfo(string cacheId)
        {
            if (_infos.TryGetValue(cacheId, out var info))
            {
                Log.Debug(i => $"GetBundleCacheInfo: {i} FOUND!", cacheId);
                return info;
            }
                
            Log.Debug(i => $"GetBundleCacheInfo: {i} NOT FOUND!", cacheId);
            return null;
        }

        public void CleanBundleCacheInfo(string cacheId)
        {
            Log.Debug(i => $"CleanBundleCacheInfo: {i}", cacheId);

            _infos.Remove(cacheId);
        }

        public void RecordCacheInfo(string cacheId, string bundleName, string url, uint version)
        {
            var info = GetBundleCacheInfo(cacheId);
            if (info != null)
            {
                // If something regarding the asset bundle changed, we record it.
                if (info.BundleName != bundleName || info.BundleUrl != url || info.BundleVersion != version)
                {
                    Log.Debug("RecordCacheInfo: Updating cache info.");
                    
                    info.Update(bundleName, url, version);
                    SaveCacheInfo();
                }
                else
                {
                    Log.Debug("RecordCacheInfo: No info changed.");
                }
            }
            else
            {
                Log.Debug("RecordCacheInfo: Adding cache info.");
                
                _infos.Add(cacheId, new AssetBundleCacheInfo(cacheId, bundleName, url, version));
                SaveCacheInfo();
            }
        }

        /*
         * Private.
         */

        private void LoadCacheInfo()
        {
            Log.Debug("Loading cache info...");

            if (!File.Exists(_infosFilesPath))
            {
                Log.Debug("Cache file doesn't exist.");
                return;
            }

            var json = File.ReadAllText(_infosFilesPath);

            Log.Debug(j => $"Json: {j}", json);
            Log.Debug("Parsing...");

            var infos = JsonConvert.DeserializeObject<List<AssetBundleCacheInfo>>(json);

            foreach (var info in infos)
                _infos.Add(info.CacheId, info);

            Log.Debug("Done");
        }

        private void SaveCacheInfo()
        {
            Log.Debug("Saving...");

            try
            {
                var json = JsonConvert.SerializeObject(_infos.Values);

                Log.Debug(() => "Json: " + json);

                File.WriteAllText(_infosFilesPath, json);
            }
            catch (Exception exception)
            {
                Log.Error(exception);
                return;
            }

            Log.Debug("Done");
        }
    }
}