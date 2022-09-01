namespace Build1.PostMVC.Core.MVCS.Injection
{
    public abstract class InjectionProvider<V> : IInjectionProvider<V, Inject>
    {
        public object TakeInstance(object parent, object attribute) { return TakeInstance(parent, (Inject)attribute); }
        public void   ReturnInstance(object instance)               { ReturnInstance((V)instance); }
        
        public abstract V    TakeInstance(object parent, Inject attribute);
        public abstract void ReturnInstance(V instance);
    }
    
    public abstract class InjectionProvider<V, T> : IInjectionProvider<V, T> where T : Inject
    {
        public object TakeInstance(object parent, object attribute) { return TakeInstance(parent, (T)attribute); }
        public void   ReturnInstance(object instance)               { ReturnInstance((V)instance); }

        public abstract V    TakeInstance(object parent, T attribute);
        public abstract void ReturnInstance(V instance);
    }
}