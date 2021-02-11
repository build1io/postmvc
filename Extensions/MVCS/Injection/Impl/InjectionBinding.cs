using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal sealed class InjectionBinding : IInjectionBinding, IInjectionBindingTo, IInjectionBindingConstruct
    {
        public Type                 Key                { get; }
        public object               Value              { get; private set; }
        public InjectionBindingType BindingType        { get; private set; }
        public InjectionMode        InjectionMode      { get; private set; }
        public bool                 ToConstructOnStart { get; private set; }

        public Type InjectionAttribute { get; private set; } = typeof(Inject);

        public InjectionBinding(Type key)
        {
            Key = key;
            Value = key;
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Factory;
        }

        public void SetValue(object value)
        {
            Value = value;
        }

        public IInjectionBinding To<T>() where T : class, new()
        {
            Value = typeof(T);
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Factory;
            return this;
        }

        public IInjectionBindingByAttribute ToValue(object value)
        {
            Value = value;
            BindingType = InjectionBindingType.Value;
            InjectionMode = InjectionMode.Singleton;
            return this;
        }

        public IInjectionBindingByAttribute ToInstanceProvider<T>() where T : IInjectionInstanceProvider
        {
            Value = typeof(T);
            BindingType = InjectionBindingType.InstanceProvider;
            InjectionMode = InjectionMode.Factory;
            return this;
        }

        public void AsFactory()
        {
            InjectionMode = InjectionMode.Factory;
        }

        public IInjectionBindingConstruct AsSingleton()
        {
            InjectionMode = InjectionMode.Singleton;
            return this;
        }

        public void ConstructOnStart()
        {
            ToConstructOnStart = true;
        }

        public IInjectionBindingAs ByAttribute<T>() where T : Inject
        {
            InjectionAttribute = typeof(T);
            return this;
        }

        public IInjectionBinding ToBinding()
        {
            return this;
        }

        public override string ToString()
        {
            return Key.ToString();
        }
    }
}