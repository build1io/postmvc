namespace Build1.PostMVC.Extensions.MVCS.Injection.Api
{
    public interface IInjectionBindingAs
    {
        void                       AsFactory();
        IInjectionBindingConstruct AsSingleton();
    }
}