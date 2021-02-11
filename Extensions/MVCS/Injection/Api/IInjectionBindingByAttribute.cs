namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingByAttribute
    {
        IInjectionBindingAs ByAttribute<T>() where T : Inject;
        IInjectionBinding   ToBinding();
    }
}