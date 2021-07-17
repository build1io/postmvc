using Build1.PostMVC.Contexts;
using Build1.PostMVC.Extensions.MVCS;
using UnityEngine;

namespace Build1.PostMVC
{
    public static class PostMVC
    {
        private static IContext _rootContext;
        
        /*
         * Init.
         */
        
        public static IContext Init() 
        {
            if (_rootContext != null)
            {
                Debug.LogError("PostMVC already initialized.");
                return null;
            }
            
            _rootContext = new Context();
            _rootContext.OnStopped += OnRootStopped;
            
            return _rootContext;
        }
        
        /*
         * Static Workflow.
         */

        public static T GetInstance<T>() where T : class
        {
            return _rootContext.GetExtension<MVCSExtension>().InjectionBinder.GetInstance<T>();
        }

        public static T Construct<T>(bool triggerPostConstructors) where T : class, new()
        {
            return _rootContext.GetExtension<MVCSExtension>().InjectionBinder.Construct<T>(triggerPostConstructors);
        }

        public static T Construct<T>(T instance, bool triggerPostConstructors) where T : class
        {
            return _rootContext.GetExtension<MVCSExtension>().InjectionBinder.Construct(instance, triggerPostConstructors);
        }

        public static T Destroy<T>(T instance, bool triggerPreDestroys) where T : class
        {
            return _rootContext.GetExtension<MVCSExtension>().InjectionBinder.Destroy(instance, triggerPreDestroys);
        }

        public static object Destroy(object instance, bool triggerPreDestroys)
        {
            return _rootContext.GetExtension<MVCSExtension>().InjectionBinder.Destroy(instance, triggerPreDestroys);
        }

        /*
         * Event Handlers.
         */

        private static void OnRootStopped()
        {
            if (_rootContext == null)
                return;
            
            _rootContext.OnStopped -= OnRootStopped;
            _rootContext = null;
        }
    }
}