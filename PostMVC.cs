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