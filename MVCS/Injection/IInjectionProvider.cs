namespace Build1.PostMVC.Core.MVCS.Injection
{
    public interface IInjectionProvider
    {
        object TakeInstance(object parent, object attribute);
        void   ReturnInstance(object instance);
    }

    public interface IInjectionProvider<V, in A> : IInjectionProvider where A : Inject
    {
        V    TakeInstance(object parent, A attribute);
        void ReturnInstance(V instance);
    }
}