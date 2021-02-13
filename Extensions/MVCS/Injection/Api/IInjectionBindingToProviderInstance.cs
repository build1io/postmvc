namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToProviderInstance
    {
        IInjectionBindingToBinding  ByAttribute<T>() where T : Inject;
        IInjectionBindingToProvider ConstructProvider();
        IInjectionBindingToBinding  ConstructOnStart();
        IInjectionBinding           ToBinding();
    }
}