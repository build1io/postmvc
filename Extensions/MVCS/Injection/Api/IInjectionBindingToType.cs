namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingToType
    {
        IInjectionBindingToBinding              AsFactory();
        IInjectionBindingToTypeConstructOnStart AsSingleton();
        IInjectionBindingToBinding              ConstructOnStart();
        IInjectionBinding                       ToBinding();
    }
}