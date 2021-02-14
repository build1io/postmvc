using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;

namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public abstract class InjectionProvider<T> : IInjectionProvider<T> where T : Inject
    {
        public object GetInstance(object parent, Inject attribute)
        {
            if (parent == null)
                throw new InjectionException(InjectionExceptionType.InstanceIsMissing);
            
            if (attribute == null)
                throw new InjectionException(InjectionExceptionType.InjectionAttributeIsMissing);
            
            if (!(attribute is Logger))
                throw new InjectionException(InjectionExceptionType.InjectionAttributeTypeMismatch);
            
            return GetInstance(parent, (T)attribute);
        }
        
        public abstract object GetInstance(object parent, T attribute);
        
        public abstract void ReturnInstance(object instance);
    }
}