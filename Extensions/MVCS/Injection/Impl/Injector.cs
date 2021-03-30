using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    internal sealed class Injector : IInjector
    {
        private const int CircularDependencyLimit = 10;

        private readonly IInjectionBinder                   _binder;
        private readonly IReflector<MVCSItemReflectionInfo> _reflector;
        private readonly HashSet<object>                    _constructed;
        private readonly Dictionary<IInjectionBinding, int> _circularDependencyCounters;

        public Injector(IInjectionBinder binder)
        {
            _binder = binder;
            _reflector = new Reflector<MVCSItemReflectionInfo>();
            _constructed = new HashSet<object>();
            _circularDependencyCounters = new Dictionary<IInjectionBinding, int>();
        }

        /*
         * Instances.
         */

        public object GetInstance(IInjectionBinding binding, object callerInstance, IInjectionInfo injectionInfo)
        {
            if (binding == null)
                throw new InjectionException(InjectionExceptionType.BindingIsMissing, callerInstance);
            IncrementDependencyCounter(binding, callerInstance);
            var instance = GetInjectionValue(callerInstance, binding, injectionInfo);
            DecrementDependencyCounter(binding);
            return instance;
        }

        /*
         * Construction & Destroying.
         */

        public void Construct(object instance, bool triggerPostConstructors)
        {
            ValidateInstance(instance, out var type);

            var info = _reflector.Get(type);
            Inject(instance, info);
            if (triggerPostConstructors)
                PostConstruct(instance, info);
        }

        public void Destroy(object instance, bool triggerPreDestroys)
        {
            ValidateInstance(instance, out var type);

            var info = _reflector.Get(type);

            if (triggerPreDestroys)
                PreDestroy(instance, info);

            UnInject(instance, info);
        }

        /*
         * Binding Value.
         */

        public void DisposeBindingValue(IInjectionBinding binding)
        {
            switch (binding.BindingType)
            {
                case InjectionBindingType.InstanceProvider when binding.InjectionMode == InjectionMode.Factory:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        Destroy(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }

                case InjectionBindingType.Value when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        Destroy(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        Destroy(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }
            }
        }

        /*
         * Validation.
         */

        private void ValidateInstance(object instance, out Type type)
        {
            if (instance == null)
                throw new InjectionException(InjectionExceptionType.InstanceIsMissing);

            type = instance.GetType();
            if (!CheckTypeCanBeConstructed(type))
                throw new InjectionException(InjectionExceptionType.InstanceIsOfPrimitiveType);
        }

        /*
         * Injections.
         */

        private void Inject(object instance, IMVCSItemReflectionInfo info)
        {
            foreach (var injection in info.Injections)
            {
                var binding = _binder.GetBinding(injection.PropertyInfo.PropertyType);
                if (binding == null)
                    throw new InjectionException(InjectionExceptionType.BindingIsMissing, injection);

                var value = GetInstance(binding, instance, injection);
                if (!injection.PropertyInfo.PropertyType.IsInstanceOfType(value))
                    throw new InjectionException(InjectionExceptionType.InjectionTypeMismatch, injection, value.GetType().Name);

                injection.PropertyInfo.SetValue(instance, value, null);
            }
        }

        private object GetInjectionValue(object instance, IInjectionBinding binding, IInjectionInfo injectionInfo)
        {
            switch (binding.BindingType)
            {
                case InjectionBindingType.InstanceProvider when binding.InjectionMode == InjectionMode.Factory:
                {
                    var instanceProvider = binding.Value is Type type
                                               ? (IInjectionProvider)Activator.CreateInstance(type)
                                               : (IInjectionProvider)binding.Value;

                    if (binding.ToConstruct && !CheckIsConstructed(instanceProvider))
                    {
                        Construct(instanceProvider, true);
                        binding.SetValue(instanceProvider);
                        MarkConstructed(binding.Value);
                    }

                    return instanceProvider.GetInstance(instance, injectionInfo.Attribute);
                }

                case InjectionBindingType.Value when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && !CheckIsConstructed(binding.Value))
                    {
                        Construct(binding.Value, true);
                        MarkConstructed(binding.Value);
                    }

                    return binding.Value;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Factory:
                {
                    var value = Activator.CreateInstance((Type)binding.Value);
                    Construct(value, true);
                    return value;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && !CheckIsConstructed(binding.Value))
                    {
                        var value = Activator.CreateInstance((Type)binding.Value);
                        Construct(value, true);
                        binding.SetValue(value);
                        MarkConstructed(binding.Value);
                    }

                    return binding.Value;
                }

                default:
                    throw new InjectionException(InjectionExceptionType.ValueNotProvided);
            }
        }

        private void UnInject(object instance, IMVCSItemReflectionInfo info)
        {
            foreach (var injection in info.Injections)
            {
                var type = injection.PropertyInfo.PropertyType;
                var binding = _binder.GetBinding(type);
                if (binding != null)
                    DestroyInjectedValue(instance, binding, injection);

                // No need to reset value type values as it'll be consuming resources.
                if (!type.IsValueType)
                    injection.PropertyInfo.SetValue(instance, null, null);
            }
        }

        private void DestroyInjectedValue(object instance, IInjectionBinding binding, IInjectionInfo injection)
        {
            var value = injection.PropertyInfo.GetValue(instance);
            switch (binding.BindingType)
            {
                case InjectionBindingType.InstanceProvider when binding.InjectionMode == InjectionMode.Factory:
                    // Provider constructed (if configured) on the first instance inject.
                    // Provider is destroyed when it's binding is destroyed.
                    ((IInjectionProvider)binding.Value).ReturnInstance(instance);
                    return;
                case InjectionBindingType.Value when binding.InjectionMode == InjectionMode.Singleton:
                    // Value is constructed (if configured) on the first instance inject.
                    // Value is destroyed when it's binding is destroyed.
                    return;
                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Factory:
                    // Factory instance is destroyed when it's un injected.
                    Destroy(value, true);
                    return;
                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Singleton:
                    // Singleton must stay alive even if all dependent parts un injected it.
                    // Singleton will be destroyed when it's binding is destroyed.
                    return;
                default:
                    throw new InjectionException(InjectionExceptionType.ValueNotDestroyed);
            }
        }

        /*
         * PostConstructs & PreDestroys.
         */

        private void PostConstruct(object instance, IMVCSItemReflectionInfo info)
        {
            if (info.PostConstructors.Count == 0)
                return;
            foreach (var method in info.PostConstructors)
                method.Invoke(instance, null);
        }

        private void PreDestroy(object instance, IMVCSItemReflectionInfo info)
        {
            if (info.PreDestroys.Count == 0)
                return;
            foreach (var method in info.PreDestroys)
                method.Invoke(instance, null);
        }

        /*
         * Circular Dependencies.
         */

        private void IncrementDependencyCounter(IInjectionBinding binding, object instance)
        {
            if (!_circularDependencyCounters.ContainsKey(binding))
                _circularDependencyCounters.Add(binding, 1);
            else
                _circularDependencyCounters[binding]++;

            if (_circularDependencyCounters[binding] > CircularDependencyLimit)
                throw new InjectionException(InjectionExceptionType.CircularDependency, binding, instance);
        }

        private void DecrementDependencyCounter(IInjectionBinding binding)
        {
            if (!_circularDependencyCounters.ContainsKey(binding))
                throw new InjectionException(InjectionExceptionType.CircularDependencyIsCounterMissing, binding);

            if (_circularDependencyCounters[binding] == 0)
                throw new InjectionException(InjectionExceptionType.CircularDependencyCounterIsAlreadyZero, binding);

            _circularDependencyCounters[binding]--;

            if (_circularDependencyCounters[binding] == 0)
                _circularDependencyCounters.Remove(binding);
        }

        /*
         * Constructed Instances.
         */

        private bool CheckIsConstructed(object instance)
        {
            return _constructed.Contains(instance);
        }

        private void MarkConstructed(object instance)   { _constructed.Add(instance); }
        private void UnmarkConstructed(object instance) { _constructed.Remove(instance); }

        /*
         * Static.
         */

        public static bool CheckTypeCanBeConstructed(Type type)
        {
            return !type.IsPrimitive && type != typeof(Decimal) && type != typeof(string);
        }
    }
}