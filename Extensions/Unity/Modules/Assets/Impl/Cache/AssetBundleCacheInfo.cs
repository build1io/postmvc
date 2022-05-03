using Newtonsoft.Json;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Cache
{
    public sealed class AssetBundleCacheInfo
    {
        [JsonProperty("i")] public string CacheId         { get; }
        [JsonProperty("n")] public string BundleName      { get; private set; }
        [JsonProperty("u")] public string BundleUrl       { get; private set; }
        [JsonProperty("v")] public uint   BundleVersion   { get; private set; }
        [JsonProperty("s")] public ulong  BundleSizeBytes { get; private set; }

        public AssetBundleCacheInfo(string cacheId, string bundleName, string bundleUrl, uint bundleVersion, ulong bundleSizeBytes)
        {
            CacheId = cacheId;

            Update(bundleName, bundleUrl, bundleVersion, bundleSizeBytes);
        }

        public void Update(string bundleName, string bundleUrl, uint bundleVersion, ulong bundleSizeBytes)
        {
            BundleName = bundleName;
            BundleUrl = bundleUrl;
            BundleVersion = bundleVersion;
            BundleSizeBytes = bundleSizeBytes;
        }
    }
}