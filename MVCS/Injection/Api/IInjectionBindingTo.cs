namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    public interface IInjectionBindingTo
    {
        IInjectionBindingToValue To(object value);
        IInjectionBindingToValue ToValue(object value);

        IInjectionBindingToProvider         ToProvider<T>() where T : IInjectionProvider, new();
        IInjectionBindingToProviderInstance ToProvider(IInjectionProvider provider);
        
        IInjectionBindingToBinding              AsFactory();
        IInjectionBindingToTypeConstructOnStart AsSingleton();
        IInjectionBinding                       ToBinding();
    }

    public interface IInjectionBindingTo<in I> : IInjectionBindingTo
    {
        IInjectionBindingToType To<T>() where T : I, new();
    }
}