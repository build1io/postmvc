namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionProvider
    {
        object GetInstance(object parent, Inject attribute);
        void   ReturnInstance(object instance);
    }
    
    public interface IInjectionProvider<in T> : IInjectionProvider where T : Inject
    {
        object GetInstance(object parent, T attribute);
    }
}