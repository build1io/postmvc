using System;
using System.Collections;
using UnityEngine;
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

        public abstract void LoadAssetBundleAsync(string bundleName,
                                                  Action<UnityEngine.AssetBundle> onComplete,
                                                  Action<AssetsException> onError);

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