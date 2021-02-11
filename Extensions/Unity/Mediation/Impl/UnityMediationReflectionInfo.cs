using System;
using System.Collections.Generic;
using System.Reflection;
using Build1.PostMVC.Extensions.Unity.Mediation.Api;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    internal sealed class UnityMediationReflectionInfo : IUnityMediationReflectionInfo
    {
        private readonly IDictionary<Type, IList<MethodInfo>> _methods;

        public UnityMediationReflectionInfo()
        {
            _methods = new Dictionary<Type, IList<MethodInfo>>();
        }

        public IReflectionInfo Build(Type type)
        {
            _methods.Add(typeof(Start), GetMethodList<Start>(type));
            _methods.Add(typeof(OnEnable), GetMethodList<OnEnable>(type));
            _methods.Add(typeof(OnDisable), GetMethodList<OnDisable>(type));
            return this;
        }

        public IList<MethodInfo> GetMethodsInfos<T>() where T : Attribute
        {
            return _methods[typeof(T)];
        }

        /*
         * Static.
         */
        
        private static List<MethodInfo> GetMethodList<T>(IReflect type) where T : Attribute
        {
            var methods = type.GetMethods(BindingFlags.FlattenHierarchy |
                                          BindingFlags.Public |
                                          BindingFlags.NonPublic |
                                          BindingFlags.Instance |
                                          BindingFlags.InvokeMethod);
            
            var methodList = new List<MethodInfo>();
            foreach (var method in methods)
            {
                if (method.GetCustomAttributes(typeof(T), true).Length > 0)
                    methodList.Add(method);
            }

            return methodList;
        }
    }
}