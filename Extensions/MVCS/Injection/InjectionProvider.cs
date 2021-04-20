namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public abstract class InjectionProvider<T, V> : IInjectionProvider<T, V> where T : Inject
    {
        public object TakeInstance(object parent, object attribute) { return TakeInstance(parent, (T)attribute); }
        public void   ReturnInstance(object instance)               { ReturnInstance((V)instance); }

        public abstract V    TakeInstance(object parent, T attribute);
        public abstract void ReturnInstance(V instance);
    }
}