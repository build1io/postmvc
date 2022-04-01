using System;
using System.IO;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentWebGL : AssetsAgentBase
    {
        public override void LoadAsync(AssetBundleInfo info,
                                       Action<AssetBundleInfo, float, ulong> onProgress,
                                       Action<AssetBundleInfo, AssetBundle> onComplete,
                                       Action<AssetBundleInfo, AssetsException> onError)
        {
            if (info.IsEmbedBundle)
                info.OverrideBundleUrl(Path.Combine(Application.streamingAssetsPath, info.BundleId));
            else if (!info.IsRemoteBundle)
                throw new AssetsException(AssetsExceptionType.UnknownBundleType);

            StartCoroutine(LoadRemoteAssetBundleCoroutine(info, onProgress, onComplete, onError));
        }
    }
}