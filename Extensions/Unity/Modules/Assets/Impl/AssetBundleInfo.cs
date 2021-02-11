using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl
{
    internal sealed class AssetBundleInfo
    {
        public bool        IsLoaded => Bundle != null;
        public AssetBundle Bundle   { get; private set; }

        public readonly int      bundleId;
        public readonly string   bundleName;
        public readonly string[] atlasesIds;

        public AssetBundleInfo(int bundleId, string bundleName, string[] atlasesIds)
        {
            this.bundleId = bundleId;
            this.bundleName = bundleName;
            this.atlasesIds = atlasesIds;
        }

        public void SetBundle(AssetBundle bundle)
        {
            Bundle = bundle;
        }

        public override string ToString()
        {
            return $"id: {bundleId} name: {bundleName}";
        }
    }
}