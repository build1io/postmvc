using System;
using Build1.PostMVC.Contexts;
using Build1.PostMVC.Extensions.ContextView.Context;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Contexts
{
    internal sealed class ContextUnityView : MonoBehaviour, IContextView
    {
        public IContext Context { get; private set; }
        public object   ViewRaw => this;

        public void SetContext(IContext context)
        {
            Context = context;
        }

        public T As<T>() where T : class
        {
            if (typeof(T) == typeof(MonoBehaviour))
                return this as T;
            if (typeof(T) == typeof(GameObject))
                return gameObject as T;
            if (typeof(T) == typeof(Transform))
                return transform as T;
            throw new Exception($"Incompatible required type for view [{typeof(T).FullName}].");
        }

        #if UNITY_EDITOR
        
        /*
         * Added for debug needs, to test disposal processes.
         */
        public void OnDisable()
        {
            Context.Stop();
        }
        
        #endif
    }
}