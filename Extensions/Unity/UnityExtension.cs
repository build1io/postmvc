using System;
using Build1.PostMVC.Extensions.ContextView;
using Build1.PostMVC.Extensions.ContextView.Contexts;
using Build1.PostMVC.Extensions.MVCS;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.Unity.Contexts;
using Build1.PostMVC.Extensions.Unity.Events.Impl;
using Build1.PostMVC.Extensions.Unity.Mediation.Api;
using Build1.PostMVC.Extensions.Unity.Mediation.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Agents.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.App;
using Build1.PostMVC.Extensions.Unity.Modules.App.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Assets.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Async;
using Build1.PostMVC.Extensions.Unity.Modules.Async.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Coroutines;
using Build1.PostMVC.Extensions.Unity.Modules.Coroutines.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.Update;
using Build1.PostMVC.Extensions.Unity.Modules.Update.Impl;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity
{
    public sealed class UnityExtension : Extension
    {
        public const string RootGameObjectName = "[PostMVC]";

        private GameObject _contextViewGameObject;
        
        /*
         * Public.
         */

        public override void Initialize()
        {
            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Bind<IUnityViewEventProcessor, UnityViewEventProcessor>();
            injectionBinder.Bind<ILog, LogProvider, LogAttribute>(); // TODO: move to PostMVCUnityApp module.
            injectionBinder.Bind<IAgentsController, AgentsController>();
            injectionBinder.Bind<IAppController, AppController>().ConstructOnStart();
            injectionBinder.Bind<IAssetsController, AssetsController>();
            injectionBinder.Bind<IAsyncResolver, AsyncResolver>().ConstructOnStart();
            injectionBinder.Bind<ICoroutineProvider, CoroutineProvider>();
            injectionBinder.Bind<IUpdateController, UpdateController>();
            injectionBinder.Rebind<IEventBus, EventBusUnity>();
        }

        public override void Setup()
        {
            object view = null;
            if (Context.TryGetExtension<ContextViewExtension>(out var viewExtension))
                view = viewExtension.View;

            var viewGameObject = GetViewGameObject(view);
            var viewAgent = viewGameObject.AddComponent<ContextUnityView>();
            viewAgent.SetContext(Context);

            Object.DontDestroyOnLoad(viewGameObject);

            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Rebind<IContextView>().ToValue(viewAgent);

            if (Context.IsRootContext)
            {
                viewGameObject.name = RootGameObjectName;
                if (view != null)
                    Object.Destroy(viewGameObject.GetComponent(view.GetType()));
            }
            else
            {
                viewGameObject.name = GetType().Name;
                if (RootContext.TryGetExtension<ContextViewExtension>(out var rootViewExtension))
                    viewGameObject.transform.SetParent(((GameObject)rootViewExtension.View)?.transform);
            }

            _contextViewGameObject = viewGameObject;
        }

        public override void Dispose()
        {
            // We don't need to unbind anything. MVCSExtension does it.
            // But we need to remove Context View Game Object.
            Object.Destroy(_contextViewGameObject);
            _contextViewGameObject = null;
        }

        /*
         * Private.
         */

        private static GameObject GetViewGameObject(object view)
        {
            switch (view)
            {
                case null:                    return new GameObject();
                case GameObject gameObject:   return gameObject;
                case MonoBehaviour behaviour: return behaviour.gameObject;
                default:
                    throw new Exception($"Specified view is not a GameObject or MonoBehavior. [{view.GetType()}]");
            }
        }
    }
}