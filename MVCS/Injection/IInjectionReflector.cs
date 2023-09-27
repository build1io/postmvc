using System;
using Build1.PostMVC.Core.MVCS.Injection.Impl;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    public interface IInjectionReflector
    {
        event Func<Type, MVCSItemReflectionInfo, MVCSItemReflectionInfo> OnReflectionInfoPreparing;
        
        MVCSItemReflectionInfo Get<T>();
        MVCSItemReflectionInfo Get(Type type);
        
        void Dispose<Y>();
        void Dispose(Type type);
    }
}