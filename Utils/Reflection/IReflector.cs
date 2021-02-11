using System;

namespace Build1.PostMVC.Utils.Reflection
{
    internal interface IReflector<out T> where T : class, IReflectionInfo, new()
    {
        T Get<Y>();
        T Get(Type type);
        
        void Dispose<Y>();
        void Dispose(Type type);
    }
}