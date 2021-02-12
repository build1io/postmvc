namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToProvider
    {
        IInjectionBindingToBinding ByAttribute<T>() where T : Inject;
        IInjectionBinding          ToBinding();
    }
}