using Newtonsoft.Json;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Cache
{
    public sealed class AssetBundleCacheInfo
    {
        [JsonProperty("i")] public string CacheId       { get; }
        [JsonProperty("n")] public string BundleName    { get; private set; }
        [JsonProperty("u")] public string BundleUrl     { get; private set; }
        [JsonProperty("v")] public uint   BundleVersion { get; private set; }

        public AssetBundleCacheInfo(string cacheId, string bundleName, string bundleUrl, uint bundleVersion)
        {
            CacheId = cacheId;
            
            Update(bundleName, bundleUrl, bundleVersion);
        }

        public void Update(string bundleName, string bundleUrl, uint bundleVersion)
        {
            BundleName = bundleName;
            BundleUrl = bundleUrl;
            BundleVersion = bundleVersion;
        }
    }
}