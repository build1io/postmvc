namespace Build1.PostMVC.Core.MVCS.Injection.Api
{
    public interface IInjectionBindingToProvider
    {
        IInjectionBindingToBinding ByAttribute<T>() where T : Inject;
        IInjectionBinding          ToBinding();
    }
}