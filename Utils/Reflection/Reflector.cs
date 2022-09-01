using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Core.Utils.Reflection
{
    public sealed class Reflector<T> : IReflector<T> where T : class, IReflectionInfo, new()
    {
        private readonly Dictionary<Type, T> _infos;
        
        public Reflector()
        {
            _infos = new Dictionary<Type, T>();
        }
        
        /*
         * Public.
         */
        
        public T Get<Y>()
        {
            return Get(typeof(Y));
        }
        
        public T Get(Type type)
        {
            if (_infos.TryGetValue(type, out var info))
                return info;
            
            info = new T();
            info.Build(type);
            
            _infos.Add(type, info);
            return info;
        }

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