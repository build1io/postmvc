using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Extensions.MVCS.Injection.Api;

namespace Build1.PostMVC.Extensions.MVCS.Injection.Impl
{
    public sealed class InjectionBinder : IInjectionBinder
    {
        private readonly Dictionary<Type, IInjectionBinding> _bindings;
        private readonly IInjector                           _injector;

        public InjectionBinder()
        {
            _bindings = new Dictionary<Type, IInjectionBinding>();
            _injector = new Injector(this);
        }

        /*
         * Bindings.
         */

        public IInjectionBindingTo Bind<T>() where T : class
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

        public IInjectionBindingTo Rebind<T>() where T : class
        {
            return Rebind(typeof(T));
        }

        public IInjectionBindingTo Rebind(Type type)
        {
            Unbind(type);
            return Bind(type);
        }

        public void Unbind<T>() where T : class
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

        public IInjectionBinding GetBinding<T>() where T : class
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
            _injector.DisposeBindingValue(binding);
        }

        /*
         * Instances.
         */

        public T GetInstance<T>() where T : class
        {
            return (T)GetInstance(GetBinding<T>());
        }

        public object GetInstance(Type key)
        {
            return GetInstance(GetBinding(key));
        }

        public object GetInstance(IInjectionBinding binding)
        {
            return _injector.GetInstance(binding, this, null);
        }

        /*
         * Construction / Deconstruction.
         */

        public T Construct<T>(T instance, bool triggerPostConstructors) where T : class
        {
            _injector.Construct(instance, triggerPostConstructors);
            return instance;
        }
        
        public object Construct(object instance, bool triggerPostConstructors)
        {
            _injector.Construct(instance, triggerPostConstructors);
            return instance;
        }

        public T Destroy<T>(T instance, bool triggerPreDestroys) where T : class
        {
            _injector.Destroy(instance, triggerPreDestroys);
            return instance;
        }
        
        public object Destroy(object instance, bool triggerPreDestroys)
        {
            _injector.Destroy(instance, triggerPreDestroys);
            return instance;
        }
    }
}