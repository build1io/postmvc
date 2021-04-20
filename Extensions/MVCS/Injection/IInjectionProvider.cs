namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionProvider
    {
        object TakeInstance(object parent, object attribute);
        void   ReturnInstance(object instance);
    }

    public interface IInjectionProvider<in I, V> : IInjectionProvider where I : Inject
    {
        V    TakeInstance(object parent, I attribute);
        void ReturnInstance(V instance);
    }
}