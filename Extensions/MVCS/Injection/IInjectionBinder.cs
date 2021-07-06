using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection
{
    public interface IInjectionBinder
    {
        IInjectionBindingTo Bind<T>() where T : class;
        IInjectionBindingTo Bind(Type type);
        void                Bind(IInjectionBinding binding);

        IInjectionBindingTo Rebind<T>() where T : class;
        IInjectionBindingTo Rebind(Type type);

        void Unbind<T>() where T : class;
        void Unbind(Type type);
        void Unbind(IInjectionBinding binding);
        void UnbindAll();

        IInjectionBinding GetBinding<T>() where T : class;
        IInjectionBinding GetBinding(Type key);

        void ForEachBinding(Action<IInjectionBinding> handler);

        T      GetInstance<T>() where T : class;
        object GetInstance(Type key);
        object GetInstance(IInjectionBinding binding);

        T      Construct<T>(bool triggerPostConstructors) where T : class, new();
        T      Construct<T>(T instance, bool triggerPostConstructors) where T : class;
        object Construct(object instance, bool triggerPostConstructors);

        T      Destroy<T>(T instance, bool triggerPreDestroys) where T : class;
        object Destroy(object instance, bool triggerPreDestroys);
    }
}