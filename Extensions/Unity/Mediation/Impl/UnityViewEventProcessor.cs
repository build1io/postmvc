using System;
using Build1.PostMVC.Extensions.Unity.Mediation.Api;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    internal sealed class UnityViewEventProcessor : IUnityViewEventProcessor
    {
        private readonly IReflector<UnityMediationReflectionInfo> _reflector;
        
        public UnityViewEventProcessor()
        {
            _reflector = new Reflector<UnityMediationReflectionInfo>();
        }

        public void ProcessStart(IUnityView view)
        {
            ProcessInstanceMethods<Start>(view);
            ProcessInstanceMethods<Start>(view.Mediator);
        }

        public void ProcessOnEnable(IUnityView view)
        {
            ProcessInstanceMethods<OnEnable>(view);
            ProcessInstanceMethods<OnEnable>(view.Mediator);
        }

        public void ProcessOnDisable(IUnityView view)
        {
            ProcessInstanceMethods<OnDisable>(view);
            ProcessInstanceMethods<OnDisable>(view.Mediator);
        }
        
        /*
         * Private.
         */

        private void ProcessInstanceMethods<T>(object instance) where T : Attribute
        {
            if (instance == null)
                return;
            
            var info = _reflector.Get(instance.GetType());
            var methods = info.GetMethodsInfos<T>();
            if (methods.Count == 0)
                return;
            
            foreach (var method in methods)
                method.Invoke(instance, null);
        }
    }
}