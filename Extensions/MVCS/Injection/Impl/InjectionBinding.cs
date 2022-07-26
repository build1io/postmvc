using System;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal class InjectionBinding : IInjectionBinding, IInjectionBindingTo, IInjectionBindingToBinding,
                                      IInjectionBindingToProvider, IInjectionBindingToProviderInstance,
                                      IInjectionBindingToType, IInjectionBindingToTypeConstructOnStart,
                                      IInjectionBindingToValue, IInjectionBindingToValueConstruct
    {
        public Type                 Key                { get; }
        public object               Value              { get; protected set; }
        public InjectionBindingType BindingType        { get; protected set; }
        public InjectionMode        InjectionMode      { get; protected set; }
        public bool                 ToConstruct        { get; protected set; }
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
            ToConstruct = InjectionBinder.CheckTypeCanBeConstructed(Value.GetType());
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

    internal sealed class InjectionBinding<T> : InjectionBinding, IInjectionBindingTo<T>
    {
        public InjectionBinding(Type key) : base(key)
        {
        }
        
        public IInjectionBindingToType To<I>() where I : T, new()
        {
            Value = typeof(I);
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Factory;
            ToConstruct = true;
            return this;
        }
    }
}