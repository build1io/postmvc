using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Core.MVCS.Injection.Impl
{
    public sealed class InjectionReflector : IInjectionReflector
    {
        private readonly Dictionary<Type, MVCSItemReflectionInfo> _infos;
        private readonly MVCSItemReflectionInfo                   _defaultEmpty;

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
            }
            else
            {
                info = _defaultEmpty;
            }

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