using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Core.MVCS.Injection.Api;

namespace Build1.PostMVC.Core.MVCS.Injection.Impl
{
    public sealed class InjectionBinder : IInjectionBinder
    {
        private const int CircularDependencyLimit = 10;

        private readonly Dictionary<Type, IInjectionBinding> _bindings;
        private readonly IInjectionReflector                 _reflector;
        private readonly HashSet<object>                     _constructed;
        private readonly Dictionary<IInjectionBinding, int>  _circularDependencyCounters;

        public InjectionBinder() : this(new InjectionReflector())
        {
        }
        
        public InjectionBinder(IInjectionReflector reflector)
        {
            _bindings = new Dictionary<Type, IInjectionBinding>();
            _reflector = reflector;
            _constructed = new HashSet<object>();
            _circularDependencyCounters = new Dictionary<IInjectionBinding, int>();
        }

        /*
         * Bindings.
         */

        public IInjectionBindingTo<T> Bind<T>()
        {
            var type = typeof(T);
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding<T>(type);
            _bindings.Add(type, binding);
            return binding;
        }

        public IInjectionBindingTo Bind(Type type)
        {
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding(type);
            _bindings.Add(type, binding);
            return binding;
        }

        public IInjectionBindingToType Bind<T, V>() where V : T, new()
        {
            var type = typeof(T);
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding<T>(type, typeof(V));
            _bindings.Add(type, binding);
            return binding;
        }

        public IInjectionBindingToValue Bind<T>(T value)
        {
            var type = typeof(T);
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding<T>(type, value);
            _bindings.Add(type, binding);
            return binding;
        }

        public IInjectionBindingToValue Bind<V>(Type type, V value)
        {
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding(type, value);
            _bindings.Add(type, binding);
            return binding;
        }

        public IInjectionBindingToBinding Bind<V, P, A>() where P : IInjectionProvider<V, A>, new()
                                                          where A : Inject
        {
            var type = typeof(V);
            if (_bindings.ContainsKey(type))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, type.FullName);
            var binding = new InjectionBinding<V, P, A>(type);
            _bindings.Add(type, binding);
            return binding;
        }

        public void Bind(IInjectionBinding binding)
        {
            if (_bindings.ContainsKey(binding.Key))
                throw new BindingException(BindingExceptionType.BindingAlreadyRegistered, binding.Key.FullName);
            _bindings.Add(binding.Key, binding);
        }

        /*
         * Rebinding.
         */

        public IInjectionBindingTo<T> Rebind<T>()
        {
            Unbind(typeof(T));
            return Bind<T>();
        }

        public IInjectionBindingTo Rebind(Type type)
        {
            Unbind(type);
            return Bind(type);
        }

        public IInjectionBindingToType Rebind<T, V>() where V : T, new()
        {
            Unbind(typeof(T));
            return Bind<T, V>();
        }

        public IInjectionBindingToValue Rebind<T>(T value)
        {
            Unbind(typeof(T));
            return Bind(value);
        }

        public IInjectionBindingToValue Rebind<V>(Type type, V value)
        {
            Unbind(type);
            return Bind(type, value);
        }

        public IInjectionBindingToBinding Rebind<V, P, A>() where P : IInjectionProvider<V, A>, new()
                                                            where A : Inject
        {
            Unbind(typeof(V));
            return Bind<V, P, A>();
        }

        /*
         * Unbinding.
         */

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
                throw new BindingException(BindingExceptionType.BindingDoesntMatch, binding);
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
                case InjectionBindingType.InstanceProvider:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        DestroyImpl(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }

                case InjectionBindingType.Value:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        DestroyImpl(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && CheckIsConstructed(binding.Value))
                    {
                        DestroyImpl(binding.Value, true);
                        UnmarkConstructed(binding.Value);
                    }

                    break;
                }
            }
        }

        /*
         * Instances.
         */

        public T Get<T>()         { return GetInstance<T>(GetBinding<T>(), this, null); }
        public T GetInstance<T>() { return GetInstance<T>(GetBinding<T>(), this, null); }

        public object Get(Type key)         { return GetInstance(GetBinding(key), this, null); }
        public object GetInstance(Type key) { return GetInstance(GetBinding(key), this, null); }

        public object Get(IInjectionBinding binding)         { return GetInstance(binding, this, null); }
        public object GetInstance(IInjectionBinding binding) { return GetInstance(binding, this, null); }

        private object GetInstance(IInjectionBinding binding, object callerInstance, IInjectionInfo injectionInfo)
        {
            if (binding == null)
                throw new BindingException(BindingExceptionType.BindingIsMissing);
            IncrementDependencyCounter(binding, callerInstance);
            var instance = GetInjectionValue(callerInstance, binding, injectionInfo);
            DecrementDependencyCounter(binding);
            return instance;
        }
        
        private T GetInstance<T>(IInjectionBinding binding, object callerInstance, IInjectionInfo injectionInfo)
        {
            if (binding == null)
                throw new BindingException(BindingExceptionType.BindingIsMissing, $"Injection key: {(typeof(T).FullName)}");
            IncrementDependencyCounter(binding, callerInstance);
            var instance = GetInjectionValue(callerInstance, binding, injectionInfo);
            DecrementDependencyCounter(binding);
            return (T)instance;
        }

        /*
         * Construction.
         */

        public T Construct<T>(bool triggerPostConstructors) where T : new()
        {
            var instance = new T();
            ConstructImpl(instance, triggerPostConstructors);
            MarkConstructed(instance);
            return instance;
        }

        public T Construct<T>(T instance, bool triggerPostConstructors)
        {
            if (!CheckIsConstructed(instance))
            {
                ConstructImpl(instance, triggerPostConstructors);
                MarkConstructed(instance);
            }

            return instance;
        }

        public object Construct(object instance, bool triggerPostConstructors)
        {
            if (!CheckIsConstructed(instance))
            {
                ConstructImpl(instance, triggerPostConstructors);
                MarkConstructed(instance);
            }

            return instance;
        }

        private void ConstructImpl(object instance, bool triggerPostConstructors)
        {
            if (instance == null)
                throw new InjectionException(InjectionExceptionType.InstanceIsMissing);

            var type = instance.GetType();
            if (!CheckTypeCanBeConstructed(type))
                throw new InjectionException(InjectionExceptionType.InstanceIsOfPrimitiveType);

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
            if (CheckIsConstructed(instance))
            {
                DestroyImpl(instance, triggerPreDestroys);
                UnmarkConstructed(instance);
            }

            return instance;
        }

        public object Destroy(object instance, bool triggerPreDestroys)
        {
            if (CheckIsConstructed(instance))
            {
                DestroyImpl(instance, triggerPreDestroys);
                UnmarkConstructed(instance);
            }

            return instance;
        }

        private void DestroyImpl(object instance, bool triggerPreDestroys)
        {
            if (instance == null)
                throw new InjectionException(InjectionExceptionType.InstanceIsMissing);

            var type = instance.GetType();
            if (!CheckTypeCanBeConstructed(type))
                throw new InjectionException(InjectionExceptionType.InstanceIsOfPrimitiveType);

            var info = _reflector.Get(type);

            if (triggerPreDestroys)
                PreDestroy(instance, info);

            UnInject(instance, info);
        }

        /*
         * Injections.
         */

        private void Inject(object instance, MVCSItemReflectionInfo info)
        {
            if (info.Injections == null)
                return;
            
            foreach (var injection in info.Injections)
            {
                var binding = GetBinding(injection.PropertyInfo.PropertyType);
                if (binding == null)
                    throw new BindingException(BindingExceptionType.BindingIsMissing, $"Binding: {injection}", $"Injection target: {instance.GetType().FullName}");

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
                case InjectionBindingType.InstanceProvider:
                {
                    var instanceProvider = binding.Value is Type type
                                               ? (IInjectionProvider)Activator.CreateInstance(type)
                                               : (IInjectionProvider)binding.Value;

                    if (binding.ToConstruct && !CheckIsConstructed(instanceProvider))
                    {
                        ConstructImpl(instanceProvider, true);
                        binding.SetValue(instanceProvider);
                        MarkConstructed(binding.Value);
                    }

                    return instanceProvider.TakeInstance(instance, injectionInfo?.Attribute);
                }

                case InjectionBindingType.Value:
                {
                    if (binding.ToConstruct && !CheckIsConstructed(binding.Value))
                    {
                        ConstructImpl(binding.Value, true);
                        MarkConstructed(binding.Value);
                    }

                    return binding.Value;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Factory:
                {
                    var type = (Type)binding.Value;

                    object value;

                    try
                    {
                        value = Activator.CreateInstance(type);
                    }
                    catch (MissingMethodException)
                    {
                        throw new InjectionException(InjectionExceptionType.ConstructingTypeCantBeInstantiated, type.FullName);
                    }

                    if (binding.ToConstruct)
                    {
                        ConstructImpl(value, true);
                        MarkConstructed(value);
                    }

                    return value;
                }

                case InjectionBindingType.Type when binding.InjectionMode == InjectionMode.Singleton:
                {
                    if (binding.ToConstruct && !CheckIsConstructed(binding.Value))
                    {
                        var type = (Type)binding.Value;

                        object value;

                        try
                        {
                            value = Activator.CreateInstance(type);
                        }
                        catch (MissingMethodException)
                        {
                            throw new InjectionException(InjectionExceptionType.ConstructingTypeCantBeInstantiated, type.FullName);
                        }

                        ConstructImpl(value, true);
                        binding.SetValue(value);
                        MarkConstructed(binding.Value);
                    }

                    return binding.Value;
                }

                default:
                    throw new InjectionException(InjectionExceptionType.ValueNotProvided);
            }
        }

        private void UnInject(object instance, MVCSItemReflectionInfo info)
        {
            if (info.Injections == null)
                return;
            
            foreach (var injection in info.Injections)
            {
                var type = injection.PropertyInfo.PropertyType;
                
                var binding = GetBinding(type);
                if (binding != null)
                    DestroyInjectedValue(instance, binding, injection);
                
                // No need to reset value type values as it'll be consuming resources.
                // This must be last as injected values from providers will be reset and will not return to providers.
                if (!type.IsValueType)
                    injection.PropertyInfo.SetValue(instance, null, null);
            }
        }

        private void DestroyInjectedValue(object instance, IInjectionBinding binding, IInjectionInfo injection)
        {
            var value = injection.PropertyInfo.GetValue(instance);
            switch (binding.BindingType)
            {
                case InjectionBindingType.InstanceProvider:
                    // Provider constructed (if configured) on the first instance inject.
                    // Provider is destroyed when it's binding is destroyed.
                    ((IInjectionProvider)binding.Value).ReturnInstance(value);
                    return;
                case InjectionBindingType.Value:
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

        private void PostConstruct(object instance, MVCSItemReflectionInfo info)
        {
            var infos = info.GetMethodInfos<PostConstruct>();
            if (infos == null)
                return;
            
            foreach (var method in infos)
                method.Invoke(instance, null);
        }

        private void PreDestroy(object instance, MVCSItemReflectionInfo info)
        {
            var infos = info.GetMethodInfos<PreDestroy>();
            if (infos == null)
                return;
            
            foreach (var method in infos)
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
            return !type.IsPrimitive && !type.IsEnum && type != typeof(Decimal) && type != typeof(string);
        }
    }
}