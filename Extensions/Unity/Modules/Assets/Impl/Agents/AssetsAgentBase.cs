using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.U2D;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal abstract class AssetsAgentBase : MonoBehaviour
    {
        public event Func<string, SpriteAtlas> AtlasRequested;

        #if UNITY_EDITOR

        private bool _safeMode;

        #endif

        /*
         * Public.
         */

        public abstract void LoadAssetBundleAsync(AssetBundleInfo bundleInfo,
                                                  Action<float> onProgress,
                                                  Action<AssetBundle> onComplete,
                                                  Action<AssetsException> onError);

        /*
         * Protected.
         */

        protected IEnumerator LoadEmbedAssetBundleCoroutine(string bundleName,
                                                            Action<float> onProgress,
                                                            Action<AssetBundle> onComplete,
                                                            Action<AssetsException> onError)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, bundleName);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);

            while (!bundleLoadRequest.isDone)
            {
                onProgress.Invoke(bundleLoadRequest.progress);
                yield return null;
            }

            onProgress.Invoke(bundleLoadRequest.progress);

            if (bundleLoadRequest.assetBundle == null)
            {
                onError.Invoke(new AssetsException(AssetsExceptionType.BundleNotFound, bundlePath));
            }
            else
            {
                onComplete.Invoke(bundleLoadRequest.assetBundle);
            }
        }

        protected IEnumerator LoadRemoteAssetBundleCoroutine(string url,
                                                             Action<float> onProgress,
                                                             Action<AssetBundle> onComplete,
                                                             Action<AssetsException> onError)
        {
            var bundleLoadRequest = UnityWebRequestAssetBundle.GetAssetBundle(url);
            bundleLoadRequest.SendWebRequest();

            while (!bundleLoadRequest.isDone)
            {
                onProgress.Invoke(bundleLoadRequest.downloadProgress);
                yield return null;
            }

            onProgress.Invoke(bundleLoadRequest.downloadProgress);

            switch (bundleLoadRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    onError.Invoke(new AssetsException(AssetsExceptionType.BundleLoadingNetworkError, bundleLoadRequest.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    onError.Invoke(new AssetsException(AssetsExceptionType.BundleLoadingHttpError, bundleLoadRequest.error));
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    onError.Invoke(new AssetsException(AssetsExceptionType.BundleLoadingProcessingError, bundleLoadRequest.error));
                    break;
                default:
                    onComplete.Invoke(DownloadHandlerAssetBundle.GetContent(bundleLoadRequest));
                    break;
            }
        }

        /*
         * Unity Events.
         */

        public void OnEnable()
        {
            #if UNITY_EDITOR

            StartCoroutine(HandleSafeModeForEditor());

            #endif

            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }

        public void OnDisable()
        {
            #if UNITY_EDITOR

            StopAllCoroutines();

            #endif

            SpriteAtlasManager.atlasRequested -= RequestAtlas;
        }

        #if UNITY_EDITOR

        IEnumerator HandleSafeModeForEditor()
        {
            _safeMode = true;

            yield return 0;

            _safeMode = false;
        }

        #endif

        /*
         * Event Handlers.
         */

        private void RequestAtlas(string atlasId, Action<SpriteAtlas> onComplete)
        {
            #if UNITY_EDITOR

            if (_safeMode)
            {
                try
                {
                    onComplete(AtlasRequested?.Invoke(atlasId));
                }
                catch
                {
                    onComplete(null);
                }

                return;
            }

            #endif

            onComplete(AtlasRequested?.Invoke(atlasId));
        }
    }
}