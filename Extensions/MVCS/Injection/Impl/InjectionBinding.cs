using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal sealed class InjectionBinding : IInjectionBinding, IInjectionBindingTo, IInjectionBindingToBinding,
                                             IInjectionBindingToProvider, IInjectionBindingToProviderInstance,
                                             IInjectionBindingToType, IInjectionBindingToTypeConstructOnStart,
                                             IInjectionBindingToValue, IInjectionBindingToValueConstruct
    {
        public Type                 Key                { get; }
        public object               Value              { get; private set; }
        public InjectionBindingType BindingType        { get; private set; }
        public InjectionMode        InjectionMode      { get; private set; }
        public bool                 ToConstruct        { get; private set; }
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

        public IInjectionBindingToType To<T>() where T : class, new()
        {
            Value = typeof(T);
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Factory;
            ToConstruct = true;
            return this;
        }

        public IInjectionBindingToValue To(object value)
        {
            Value = value;
            BindingType = InjectionBindingType.Value;
            InjectionMode = InjectionMode.Singleton;
            return this;
        }

        public IInjectionBindingToValue ToValue(object value)
        {
            Value = value;
            BindingType = InjectionBindingType.Value;
            InjectionMode = InjectionMode.Singleton;
            return this;
        }

        public IInjectionBindingToProvider ToProvider<T>() where T : IInjectionProvider, new()
        {
            Value = typeof(T);
            BindingType = InjectionBindingType.InstanceProvider;
            InjectionMode = InjectionMode.Factory;
            ToConstruct = true;
            return this;
        }

        public IInjectionBindingToProviderInstance ToProvider(IInjectionProvider provider)
        {
            Value = provider;
            BindingType = InjectionBindingType.InstanceProvider;
            InjectionMode = InjectionMode.Factory;
            return this;
        }

        public IInjectionBindingToValueConstruct ConstructValue()
        {
            ToConstruct = Injector.CheckTypeCanBeConstructed(Value.GetType());
            return this;
        }

        public IInjectionBindingToProvider ConstructProvider()
        {
            ToConstruct = true;
            return this;
        }

        public IInjectionBindingToBinding AsFactory()
        {
            InjectionMode = InjectionMode.Factory;
            return this;
        }

        public IInjectionBindingToTypeConstructOnStart AsSingleton()
        {
            InjectionMode = InjectionMode.Singleton;
            return this;
        }

        public IInjectionBindingToBinding ConstructOnStart()
        {
            ToConstruct = true;
            ToConstructOnStart = true;
            return this;
        }

        public IInjectionBindingToBinding ByAttribute<T>() where T : Inject
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