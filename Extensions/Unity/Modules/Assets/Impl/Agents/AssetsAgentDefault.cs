using System;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Cache;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentDefault : AssetsAgentBase
    {
        public override void LoadAsync(AssetBundleInfo info,
                                       Func<AssetBundleInfo, AssetBundleCacheInfo> onCacheInfoGet,
                                       Action<AssetBundleInfo> onCacheInfoClean,
                                       Action<string, AssetBundleInfo> onCacheInfoRecord,
                                       Action<AssetBundleInfo, float, ulong> onProgress,
                                       Action<AssetBundleInfo, AssetBundle> onComplete,
                                       Action<AssetBundleInfo, AssetsException> onError)
        {
            if (info.IsEmbedBundle)
                StartCoroutine(LoadEmbedAssetBundleCoroutine(info, onProgress, onComplete, onError));
            else if (info.IsRemoteBundle)
                StartCoroutine(LoadRemoteAssetBundleCoroutine(info, onCacheInfoGet, onCacheInfoClean, onCacheInfoRecord, onProgress, onComplete, onError));
            else
                throw new AssetsException(AssetsExceptionType.UnknownBundleType);
        }
    }
}