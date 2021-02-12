namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionProvider
    {
        object GetInstance(object parent, Inject attribute);
        void   ReturnInstance(object instance);
    }
}