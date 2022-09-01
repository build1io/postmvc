using System;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection
{
    public interface IInjectionBinder
    {
        IInjectionBindingTo<T>  Bind<T>();
        IInjectionBindingTo     Bind(Type type);
        IInjectionBindingToType Bind<T, V>() where V : T, new();

        IInjectionBindingToValue Bind<T>(T value);
        IInjectionBindingToValue Bind<V>(Type type, V value);

        IInjectionBindingToBinding Bind<V, P, A>() where P : IInjectionProvider<V, A>, new()
                                                   where A : Inject;

        void Bind(IInjectionBinding binding);

        IInjectionBindingTo<T>  Rebind<T>();
        IInjectionBindingTo     Rebind(Type type);
        IInjectionBindingToType Rebind<T, V>() where V : T, new();

        IInjectionBindingToValue Rebind<T>(T value);
        IInjectionBindingToValue Rebind<V>(Type type, V value);
        
        IInjectionBindingToBinding Rebind<V, P, A>() where P : IInjectionProvider<V, A>, new()
                                                     where A : Inject;

        void Unbind<T>();
        void Unbind(Type type);
        void Unbind(IInjectionBinding binding);

        IInjectionBinding GetBinding<T>();
        IInjectionBinding GetBinding(Type key);

        void ForEachBinding(Action<IInjectionBinding> handler);

        T      GetInstance<T>();
        object GetInstance(Type key);
        object GetInstance(IInjectionBinding binding);

        T      Construct<T>(bool triggerPostConstructors) where T : new();
        T      Construct<T>(T instance, bool triggerPostConstructors);
        object Construct(object instance, bool triggerPostConstructors);

        T      Destroy<T>(T instance, bool triggerPreDestroys);
        object Destroy(object instance, bool triggerPreDestroys);
    }
}