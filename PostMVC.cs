using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.Contexts;
using Build1.PostMVC.Core.Contexts.Impl;
using Build1.PostMVC.Core.MVCS;

namespace Build1.PostMVC.Core
{
    public static class PostMVC
    {
        public static int ActiveContextsCount => _contexts?.Count ?? 0;
        
        public static event Action<IContext> OnContextStarting;
        public static event Action<IContext> OnContextStarted;
        public static event Action<IContext> OnContextQuitting;
        public static event Action<IContext> OnContextStopping;
        public static event Action<IContext> OnContextStopped;

        private static IContext       _rootContext;
        private static List<IContext> _contexts;

        /*
         * Init.
         */

        public static IContext Context(string name = null)
        {
            var context = new Context(_contexts?.Count ?? 0, name, _rootContext);
            context.AddExtension<MVCSExtension>();
            context.OnStopped += OnContextStoppedListener;

            if (_contexts == null)
            {
                _contexts = new List<IContext> { context };
                _rootContext = context;
            }
            else
            {
                _contexts.Add(context);
            }

            return context;
        }
        
        public static void Stop()
        {
            if (_rootContext == null)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            for (var i = _contexts.Count - 1; i >= 0; i--)
                _contexts[i].Stop();
            
            _rootContext = null;
        }

        /*
         * Context.
         */

        internal static void OnContextStartingHandler(IContext context) { OnContextStarting?.Invoke(context); }
        internal static void OnContextStartedHandler(IContext context)  { OnContextStarted?.Invoke(context); }
        internal static void OnContextQuittingHandler(IContext context) { OnContextQuitting?.Invoke(context); }
        internal static void OnContextStoppingHandler(IContext context) { OnContextStopping?.Invoke(context); }
        internal static void OnContextStoppedHandler(IContext context)  { OnContextStopped?.Invoke(context); }

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

        private static void OnContextStoppedListener(IContext context)
        {
            if (!_contexts.Remove(context))
                return;
            
            context.OnStopped -= OnContextStoppedListener;

            if (context == _rootContext)
                _rootContext = context;
        }
    }
}