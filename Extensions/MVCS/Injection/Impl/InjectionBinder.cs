using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;
using Build1.PostMVC.Utils.Reflection;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    public sealed class InjectionBinder : IInjectionBinder
    {
        private const int CircularDependencyLimit = 10;

        private readonly Dictionary<Type, IInjectionBinding> _bindings;
        private readonly IReflector<MVCSItemReflectionInfo>  _reflector;
        private readonly HashSet<object>                     _constructed;
        private readonly Dictionary<IInjectionBinding, int>  _circularDependencyCounters;

        public InjectionBinder()
        {
            _bindings = new Dictionary<Type, IInjectionBinding>();
            _reflector = new Reflector<MVCSItemReflectionInfo>();
            _constructed = new HashSet<object>();
            _circularDependencyCounters = new Dictionary<IInjectionBinding, int>();
        }

        /*
         * Bindings.
         */

        public IInjectionBindingTo Bind<T>()
        {
            return Bind(typeof(T));
        }

        public IInjectionBindingTo Bind(Type type)
        {
            if (_bindings.ContainsKey(type))
                throw new InjectionException(InjectionExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding(type);
            _bindings.Add(type, binding);
            return binding;
        }

        public void Bind(IInjectionBinding binding)
        {
            if (_bindings.ContainsKey(binding.Key))
                throw new InjectionException(InjectionExceptionType.BindingAlreadyRegistered, binding.Key.FullName);
            _bindings.Add(binding.Key, binding);
        }

        public IInjectionBindingTo Rebind<T>()
        {
            return Rebind(typeof(T));
        }

        public IInjectionBindingTo Rebind(Type type)
        {
            Unbind(type);
            return Bind(type);
        }

        public void Unbind<T>()
        {
            Unbind(typeof(T));
        }

        public void Unbind(Type type)
        {
            if (!_bindings.TryGetValue(type, out var binding))
                return;
            _bindings.Remove(type);
            DestroyBinding(binding);
        }

        public void Unbind(IInjectionBinding binding)
        {
            if (!_bindings.TryGetValue(binding.Key, out var bindingAdded) || binding != bindingAdded)
                throw new InjectionException(InjectionExceptionType.BindingDoesntMatch, binding);
            _bindings.Remove(binding.Key);
            DestroyBinding(binding);
        }

        public void UnbindAll()
        {
            // Unbinding in reverse order to preserver the order reverse to initialization, but backwards.
            // This way no null refs are thrown during disposing.
            for (var i = _bindings.Count - 1; i >= 0; i--)
                DestroyBinding(_bindings.ElementAt(i).Value);
            _bindings.Clear();
        }

        public IInjectionBinding GetBinding<T>()
        {
            return GetBinding(typeof(T));
        }

        public IInjectionBinding GetBinding(Type key)
        {
            _bindings.TryGetValue(key, out var binding);
            return binding;
        }

        public void ForEachBinding(Action<IInjectionBinding> handler)
        {
            foreach (var binding in _bindings.Values)
                handler.Invoke(binding);
        }

        private void DestroyBinding(IInjectionBinding binding)
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
         * Instances.
         */

        public T GetInstance<T>()
        {
            return (T)GetInstance(GetBinding<T>());
        }

        public object GetInstance(Type key)
        {
            return GetInstance(GetBinding(key));
        }

        public object GetInstance(IInjectionBinding binding)
        {
            return GetInstance(binding, this, null);
        }
        
        private object GetInstance(IInjectionBinding binding, object callerInstance, IInjectionInfo injectionInfo)
        {
            if (binding == null)
                throw new InjectionException(InjectionExceptionType.BindingIsMissing, callerInstance);
            IncrementDependencyCounter(binding, callerInstance);
            var instance = GetInjectionValue(callerInstance, binding, injectionInfo);
            DecrementDependencyCounter(binding);
            return instance;
        }

        /*
         * Construction.
         */

        public T Construct<T>(bool triggerPostConstructors) where T : new()
        {
            return Construct(new T(), triggerPostConstructors);
        }

        public T Construct<T>(T instance, bool triggerPostConstructors)
        {
            ConstructImpl(instance, triggerPostConstructors);
            return instance;
        }

        public object Construct(object instance, bool triggerPostConstructors)
        {
            ConstructImpl(instance, triggerPostConstructors);
            return instance;
        }
        
        private void ConstructImpl(object instance, bool triggerPostConstructors)
        {
            ValidateInstance(instance, out var type);

            var info = _reflector.Get(type);
            Inject(instance, info);
            if (triggerPostConstructors)
                PostConstruct(instance, info);
        }

        /*
         * Destruction.
         */

        public T Destroy<T>(T instance, bool triggerPreDestroys)
        {
            DestroyImpl(instance, triggerPreDestroys);
            return instance;
        }

        public object Destroy(object instance, bool triggerPreDestroys)
        {
            DestroyImpl(instance, triggerPreDestroys);
            return instance;
        }
        
        private void DestroyImpl(object instance, bool triggerPreDestroys)
        {
            ValidateInstance(instance, out var type);

            var info = _reflector.Get(type);

            if (triggerPreDestroys)
                PreDestroy(instance, info);

            UnInject(instance, info);
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
                var binding = GetBinding(injection.PropertyInfo.PropertyType);
                if (binding == null)
                    throw new InjectionException(InjectionExceptionType.BindingIsMissing, $"Binding: {injection}", $"Injection target: {instance.GetType().FullName}");

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

                    return instanceProvider.TakeInstance(instance, injectionInfo.Attribute);
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
                var binding = GetBinding(type);
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
                    ((IInjectionProvider)binding.Value).ReturnInstance(value);
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