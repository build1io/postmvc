using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public sealed class AssetBundleInfo
    {
        public string       BundleUrl    { get; private set; }
        public List<string> AtlasesNames { get; private set; }

        public bool HasAtlases     => AtlasesNames != null && AtlasesNames.Count > 0;
        public bool IsEmbedBundle  => BundleUrl == null;
        public bool IsRemoteBundle => BundleUrl != null;
        public bool IsLoaded       => Bundle != null;

        internal UnityEngine.AssetBundle Bundle { get; private set; }

        public readonly Enum   bundleId;
        public readonly string bundleName;

        public AssetBundleInfo(Enum bundleId, string bundleName, params string[] atlasesNames)
        {
            this.bundleId = bundleId;
            this.bundleName = bundleName;

            if (atlasesNames != null)
                AtlasesNames = new List<string>(atlasesNames);
        }

        public AssetBundleInfo(Enum bundleId, string bundleName, string bundleUrl, params string[] atlasesNames)
        {
            this.bundleId = bundleId;
            this.bundleName = bundleName;

            BundleUrl = bundleUrl;
            
            if (atlasesNames != null)
                AtlasesNames = new List<string>(atlasesNames);
        }

        public AssetBundleInfo SetUrl(string url)
        {
            BundleUrl = url;
            return this;
        }

        public AssetBundleInfo AddAtlasesNames(params string[] atlasesNames)
        {
            if (AtlasesNames == null)
                AtlasesNames = new List<string>(atlasesNames);
            else
                AtlasesNames.AddRange(atlasesNames);
            
            return this;
        }

        internal void SetBundle(UnityEngine.AssetBundle bundle)
        {
            Bundle = bundle;
        }

        public string[] GetAllScenePaths() { return Bundle.GetAllScenePaths(); }
        public string[] GetAllAssetNames() { return Bundle.GetAllAssetNames(); }

        public override string ToString()
        {
            return bundleName;
        }
    }
}