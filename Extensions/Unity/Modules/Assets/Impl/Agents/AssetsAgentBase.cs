using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using UnityEngine;
using UnityEngine.U2D;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl.Agents
{
    internal abstract class AssetsAgentBase : MonoBehaviour
    {
        public event Func<string, SpriteAtlas> AtlasRequested;

        /*
         * Public.
         */

        public abstract void LoadAssetBundleAsync(string bundleName,
                                                  Action<AssetBundle> onComplete,
                                                  Action<AssetsException> onError);

        /*
         * Unity Events.
         */

        [PostConstruct]
        public void PostConstruct()
        {
            SpriteAtlasManager.atlasRequested += RequestAtlas;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            SpriteAtlasManager.atlasRequested -= RequestAtlas;
        }

        /*
         * Event Handlers.
         */

        private void RequestAtlas(string atlasId, Action<SpriteAtlas> onComplete)
        {
            onComplete(AtlasRequested?.Invoke(atlasId));
        }
    }
}