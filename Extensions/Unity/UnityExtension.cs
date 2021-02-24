using System;
using Build1.PostMVC.Extensions.ContextView;
using Build1.PostMVC.Extensions.ContextView.Context;
using Build1.PostMVC.Extensions.MVCS;
using Build1.PostMVC.Extensions.Unity.Contexts;
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
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.Update;
using Build1.PostMVC.Extensions.Unity.Modules.Update.Impl;
using UnityEngine;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using Object = UnityEngine.Object;

namespace Build1.PostMVC.Extensions.Unity
{
    public sealed class UnityExtension : Extension
    {
        public const string RootGameObjectName = "PostMVC";

        private GameObject _contextViewGameObject;
        
        /*
         * Public.
         */

        public override void OnInitialized()
        {
            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Bind<IUnityViewEventProcessor>().To<UnityViewEventProcessor>().AsSingleton();
            injectionBinder.Bind<ILogger>().ToProvider<LoggerProvider>().ByAttribute<Logger>();
            injectionBinder.Bind<IAgentsController>().To<AgentsController>().AsSingleton();
            injectionBinder.Bind<IAppController>().To<AppController>().AsSingleton().ConstructOnStart();
            injectionBinder.Bind<IAssetsController>().To<AssetsController>().AsSingleton();
            injectionBinder.Bind<IAsyncResolver>().To<AsyncResolver>().AsSingleton();
            injectionBinder.Bind<IUpdateController>().To<UpdateController>().AsSingleton();
        }

        public override void OnSetup()
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

        public override void OnDispose()
        {
            // We don't need to unbind anything, as MVCSExtension does this.
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