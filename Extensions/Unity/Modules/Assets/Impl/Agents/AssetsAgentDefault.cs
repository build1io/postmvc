using System;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentDefault : AssetsAgentBase
    {
        public override void LoadAssetBundleAsync(AssetBundleInfo bundleInfo,
                                                  Action<float> onProgress,
                                                  Action<AssetBundle> onComplete,
                                                  Action<AssetsException> onError)
        {
            if (bundleInfo.IsEmbedBundle)
                StartCoroutine(LoadEmbedAssetBundleCoroutine(bundleInfo.bundleName, onProgress, onComplete, onError));
            else if (bundleInfo.IsRemoteBundle)
                StartCoroutine(LoadRemoteAssetBundleCoroutine(bundleInfo.BundleUrl, onProgress, onComplete, onError));
            else
                throw new Exception("Unknown bundle loading method");
        }
    }
}