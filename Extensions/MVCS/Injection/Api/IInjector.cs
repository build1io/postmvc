namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    internal interface IInjector
    {
        object GetInstance(IInjectionBinding binding, object callerInstance, IInjectionInfo injectionInfo);
        
        void Construct(object instance, bool triggerPostConstructors);
        void Destroy(object instance, bool triggerPreDestroys);
        
        void DisposeBindingValue(IInjectionBinding binding);
    }
}