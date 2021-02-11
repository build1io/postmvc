namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingTo : IInjectionBindingAs
    {
        IInjectionBinding            To<T>() where T : class, new();
        IInjectionBindingByAttribute ToValue(object value);
        IInjectionBindingByAttribute ToInstanceProvider<T>() where T : IInjectionInstanceProvider;
    }
}