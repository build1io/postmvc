namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToProvider
    {
        IInjectionBindingToBinding ByAttribute<T>() where T : Inject;
        IInjectionBinding          ToBinding();
    }
}