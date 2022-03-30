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

        public abstract void LoadAssetBundleAsync(AssetBundleInfo info,
                                                  Action<AssetBundleInfo, float, ulong> onProgress,
                                                  Action<AssetBundleInfo, AssetBundle> onComplete,
                                                  Action<AssetBundleInfo, AssetsException> onError);

        /*
         * Protected.
         */

        protected IEnumerator LoadEmbedAssetBundleCoroutine(AssetBundleInfo info,
                                                            Action<AssetBundleInfo, float, ulong> onProgress,
                                                            Action<AssetBundleInfo, AssetBundle> onComplete,
                                                            Action<AssetBundleInfo, AssetsException> onError)
        {
            var bundlePath = Path.Combine(Application.streamingAssetsPath, info.BundleId);
            var bundleLoadRequest = AssetBundle.LoadFromFileAsync(bundlePath);

            while (!bundleLoadRequest.isDone)
            {
                onProgress.Invoke(info, bundleLoadRequest.progress, 0);
                yield return null;
            }

            onProgress.Invoke(info, bundleLoadRequest.progress, 0);

            if (bundleLoadRequest.assetBundle == null)
            {
                onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleNotFound, bundlePath));
            }
            else
            {
                onComplete.Invoke(info, bundleLoadRequest.assetBundle);
            }
        }

        protected IEnumerator LoadRemoteAssetBundleCoroutine(AssetBundleInfo info,
                                                             Action<AssetBundleInfo, float, ulong> onProgress,
                                                             Action<AssetBundleInfo, AssetBundle> onComplete,
                                                             Action<AssetBundleInfo, AssetsException> onError)
        {
            var bundleLoadRequest = info.IsCacheEnabled
                                        ? UnityWebRequestAssetBundle.GetAssetBundle(info.BundleUrl, info.BundleVersion, 0)
                                        : UnityWebRequestAssetBundle.GetAssetBundle(info.BundleUrl);

            bundleLoadRequest.SendWebRequest();

            while (!bundleLoadRequest.isDone)
            {
                onProgress.Invoke(info, bundleLoadRequest.downloadProgress, bundleLoadRequest.downloadedBytes);
                yield return null;
            }

            onProgress.Invoke(info, bundleLoadRequest.downloadProgress, bundleLoadRequest.downloadedBytes);

            switch (bundleLoadRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingNetworkError, bundleLoadRequest.error));
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingHttpError, bundleLoadRequest.error));
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    onError.Invoke(info, new AssetsException(AssetsExceptionType.BundleLoadingProcessingError, bundleLoadRequest.error));
                    break;
                default:
                    onComplete.Invoke(info, DownloadHandlerAssetBundle.GetContent(bundleLoadRequest));
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