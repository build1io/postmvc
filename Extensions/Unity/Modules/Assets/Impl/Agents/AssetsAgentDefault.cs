using System;
using System.Collections;
using System.IO;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentDefault : AssetsAgentBase
    {
        public override void LoadAssetBundleAsync(string bundleName,
                                                  Action<UnityEngine.AssetBundle> onComplete,
                                                  Action<AssetsException> onError)
        {
            StartCoroutine(LoadAssetBundleAsyncImpl(bundleName, onComplete, onError));
        }

        private IEnumerator LoadAssetBundleAsyncImpl(string bundleName,
                                                     Action<UnityEngine.AssetBundle> onComplete,
                                                     Action<AssetsException> onError)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
            var bundleLoadRequest = UnityEngine.AssetBundle.LoadFromFileAsync(bundlePath);

            yield return bundleLoadRequest;

            if (bundleLoadRequest.assetBundle == null)
            {
                onError?.Invoke(new AssetsException(AssetsExceptionType.BundleNotFound, bundlePath));
            }
            else
            {
                onComplete?.Invoke(bundleLoadRequest.assetBundle);
            }
        }
    }
}