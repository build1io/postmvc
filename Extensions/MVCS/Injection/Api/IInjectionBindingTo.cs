namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingTo
    {
        IInjectionBindingToValue To(object value);
        IInjectionBindingToValue ToValue(object value);

        IInjectionBindingToProvider         ToProvider<T>() where T : IInjectionProvider, new();
        IInjectionBindingToProviderInstance ToProvider(IInjectionProvider provider);
    }

    public interface IInjectionBindingTo<in I> : IInjectionBindingTo
    {
        IInjectionBindingToType To<T>() where T : I, new();
    }
}