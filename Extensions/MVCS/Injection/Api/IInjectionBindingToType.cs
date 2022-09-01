namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToType
    {
        IInjectionBindingToBinding              AsFactory();
        IInjectionBindingToTypeConstructOnStart AsSingleton();
        IInjectionBindingToBinding              ConstructOnStart();
        IInjectionBinding                       ToBinding();
    }
}