using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionBinder
    {
        IInjectionBindingTo Bind<T>();
        IInjectionBindingTo Bind(Type type);
        void                Bind(IInjectionBinding binding);

        IInjectionBindingTo Rebind<T>();
        IInjectionBindingTo Rebind(Type type);

        void Unbind<T>();
        void Unbind(Type type);
        void Unbind(IInjectionBinding binding);
        void UnbindAll();

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