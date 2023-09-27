using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Core.MVCS.Injection.Impl
{
    public sealed class InjectionReflector : IInjectionReflector
    {
        private readonly Dictionary<Type, MVCSItemReflectionInfo> _infos;
        private readonly MVCSItemReflectionInfo                   _defaultEmpty = new();
        
        public event Func<Type, MVCSItemReflectionInfo, MVCSItemReflectionInfo> OnReflectionInfoPreparing;

        public InjectionReflector()
        {
            _infos = new Dictionary<Type, MVCSItemReflectionInfo>();
        }

        /*
         * Get.
         */

        public MVCSItemReflectionInfo Get<T>()
        {
            return Get(typeof(T));
        }

        public MVCSItemReflectionInfo Get(Type type)
        {
            if (_infos.TryGetValue(type, out var info))
                return info;
            
            var injections = MVCSItemReflectionInfo.GetInjects(type);
            var postConstructs = MVCSItemReflectionInfo.GetMethodList<PostConstruct>(type);
            var preDestroys = MVCSItemReflectionInfo.GetMethodList<PreDestroy>(type);

            if (injections != null || postConstructs != null || preDestroys != null)
            {
                info = new MVCSItemReflectionInfo();
                
                if (injections != null)
                    info.SetInjectionInfos(injections);
                
                if (postConstructs != null)
                    info.AddMethodInfos<PostConstruct>(postConstructs);
                
                if (preDestroys != null)
                    info.AddMethodInfos<PreDestroy>(preDestroys);
            }

            if (OnReflectionInfoPreparing != null)
                info = OnReflectionInfoPreparing?.Invoke(type, info);
            
            info ??= _defaultEmpty;

            _infos.Add(type, info);

            return info;
        }

        /*
         * Dispose.
         */

        public void Dispose<Y>()
        {
            Dispose(typeof(Y));
        }

        public void Dispose(Type type)
        {
            _infos.Remove(type);
        }
    }
}