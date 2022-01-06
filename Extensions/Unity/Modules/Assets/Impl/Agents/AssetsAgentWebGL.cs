using System;
using System.IO;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentWebGL : AssetsAgentBase
    {
        public override void LoadAssetBundleAsync(AssetBundleInfo bundleInfo, 
                                                  Action<float> onProgress,
                                                  Action<AssetBundle> onComplete, 
                                                  Action<AssetsException> onError)
        {
            string url; 
            if (bundleInfo.IsEmbedBundle)
                url = Path.Combine(Application.streamingAssetsPath, bundleInfo.bundleName);
            else if (bundleInfo.IsRemoteBundle)
                url = bundleInfo.BundleUrl;
            else
                throw new Exception("Unknown bundle loading method");
            
            StartCoroutine(LoadRemoteAssetBundleCoroutine(url, onProgress, onComplete, onError));
        }
    }
}