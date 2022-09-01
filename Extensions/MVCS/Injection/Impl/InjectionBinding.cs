using System;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl
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
            InjectionMode = InjectionMode.Singleton;
        }

        public InjectionBinding(Type key, object value)
        {
            Key = key;
            Value = value;
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Singleton;
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
            ToConstruct = false;
            return this;
        }

        public IInjectionBindingToValue ToValue(object value)
        {
            Value = value;
            BindingType = InjectionBindingType.Value;
            InjectionMode = InjectionMode.Singleton;
            ToConstruct = false;
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
            ToConstruct = false;
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
            if (BindingType == InjectionBindingType.Type)
            {
                var type = (Type)Value;
                InjectionMode = type.IsValueType || type == typeof(string) ? InjectionMode.Factory : InjectionMode.Singleton;
            }
            else
            {
                InjectionMode = InjectionMode.Singleton;
            }

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
        public InjectionBinding(Type key) : this(key, key)
        {
        }

        public InjectionBinding(Type key, Type value) : base(key)
        {
            Value = value;
            BindingType = InjectionBindingType.Type;
            InjectionMode = value.IsValueType || value == typeof(string) ? InjectionMode.Factory : InjectionMode.Singleton;
            ToConstruct = InjectionBinder.CheckTypeCanBeConstructed(value);
        }

        public InjectionBinding(Type key, object value) : base(key)
        {
            Value = value;
            BindingType = InjectionBindingType.Value;
            InjectionMode = InjectionMode.Singleton;
            ToConstruct = false;
        }

        public IInjectionBindingToType To<I>() where I : T, new()
        {
            Value = typeof(I);
            BindingType = InjectionBindingType.Type;
            InjectionMode = InjectionMode.Singleton;
            ToConstruct = true;
            return this;
        }
    }

    internal sealed class InjectionBinding<V, P, A> : InjectionBinding where P : IInjectionProvider<V, A>, new()
                                                                       where A : Inject
    {
        public InjectionBinding(Type key) : base(key)
        {
            Value = typeof(P);
            BindingType = InjectionBindingType.InstanceProvider;
            InjectionMode = InjectionMode.Factory;
            ToConstruct = true;
        }
    }
}