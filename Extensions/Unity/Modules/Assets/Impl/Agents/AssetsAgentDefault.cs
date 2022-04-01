using System;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentDefault : AssetsAgentBase
    {
        public override void LoadAsync(AssetBundleInfo info,
                                       Action<AssetBundleInfo, float, ulong> onProgress,
                                       Action<AssetBundleInfo, AssetBundle> onComplete,
                                       Action<AssetBundleInfo, AssetsException> onError)
        {
            if (info.IsEmbedBundle)
                StartCoroutine(LoadEmbedAssetBundleCoroutine(info, onProgress, onComplete, onError));
            else if (info.IsRemoteBundle)
                StartCoroutine(LoadRemoteAssetBundleCoroutine(info, onProgress, onComplete, onError));
            else
                throw new AssetsException(AssetsExceptionType.UnknownBundleType);
        }
    }
}