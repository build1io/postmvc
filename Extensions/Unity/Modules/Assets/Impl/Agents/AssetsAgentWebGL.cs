using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal sealed class AssetsAgentWebGL : AssetsAgentBase
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
            var bundleLoadRequest = UnityWebRequestAssetBundle.GetAssetBundle(bundlePath);
            
            yield return bundleLoadRequest.SendWebRequest();
            
            if (bundleLoadRequest.isNetworkError)
            {
                onError?.Invoke(new AssetsException(AssetsExceptionType.BundleLoadingNetworkError, bundleLoadRequest.error));
            }
            else if (bundleLoadRequest.isHttpError)
            {
                onError?.Invoke(new AssetsException(AssetsExceptionType.BundleLoadingHttpError, bundleLoadRequest.error));
            }
            else
            {
                onComplete?.Invoke(DownloadHandlerAssetBundle.GetContent(bundleLoadRequest));    
            }
        }
    }
}