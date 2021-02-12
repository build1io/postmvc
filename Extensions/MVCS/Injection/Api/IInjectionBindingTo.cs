namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingTo
    {
        IInjectionBindingToType  To<T>() where T : class, new();
        IInjectionBindingToValue To(object value);
        IInjectionBindingToValue ToValue(object value);

        IInjectionBindingToProvider         ToProvider<T>() where T : IInjectionProvider, new();
        IInjectionBindingToProviderInstance ToProvider(IInjectionProvider provider);
    }
}