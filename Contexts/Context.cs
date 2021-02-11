using System;
using System.Collections.Generic;
using Build1.PostMVC.Contexts.Impl;
using Build1.PostMVC.Extensions;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Contexts
{
    public sealed class Context : IContext
    {
        public static Action<IContext> OnContextStart;
        public static Action<IContext> OnContextStop;

        public IContext RootContext { get; }

        public bool IsRootContext => this == RootContext;
        public bool IsStarted     { get; private set; }
        public bool IsStopping    { get; private set; }

        public Action OnStart { get; set; }
        public Action OnStop  { get; set; }
        
        private readonly HashSet<Type>                _extensions;
        private readonly Dictionary<Type, IExtension> _extensionInstances;

        private readonly List<Type>                _modules;
        private readonly Dictionary<Type, IModule> _moduleInstances;

        public Context()
        {
            RootContext = this;

            _extensions = new HashSet<Type>();
            _extensionInstances = new Dictionary<Type, IExtension>();

            _modules = new List<Type>();
            _moduleInstances = new Dictionary<Type, IModule>();
        }

        /*
         * Extensions.
         */

        public IContext AddExtension(IExtension extension)
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ExtensionInstallationAttemptAfterContextStart, extension.GetType().FullName);

            var type = extension.GetType();
            if (_extensionInstances.ContainsKey(type))
                throw new ContextException(ContextExceptionType.ExtensionInstanceAlreadyExists);

            _extensions.Add(type);
            _extensionInstances.Add(type, extension);
            return this;
        }

        public IContext AddExtension<T>() where T : IExtension, new()
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ExtensionInstallationAttemptAfterContextStart, typeof(T).FullName);

            _extensions.Add(typeof(T));
            return this;
        }

        public bool HasExtension<T>() where T : IExtension
        {
            return _extensions.Contains(typeof(T));
        }

        public T GetExtension<T>() where T : IExtension
        {
            if (_extensionInstances.TryGetValue(typeof(T), out var extension))
                return (T)extension;
            throw new ContextException(ContextExceptionType.ExtensionInstanceNotFound);
        }

        public bool TryGetExtension<T>(out T extension) where T : IExtension
        {
            if (_extensionInstances.TryGetValue(typeof(T), out var extensionInstance))
            {
                extension = (T)extensionInstance;
                return true;
            }

            extension = default;
            return false;
        }

        private void InitializeExtensions()
        {
            foreach (var extensionType in _extensions)
            {
                if (!_extensionInstances.TryGetValue(extensionType, out var extension))
                {
                    extension = (IExtension)Activator.CreateInstance(extensionType);
                    _extensionInstances.Add(extensionType, extension);
                }

                extension.SetContext(this);
                extension.SetRootContext(RootContext);
            }

            foreach (var extension in _extensionInstances.Values)
                extension.OnInitialized();

            foreach (var extension in _extensionInstances.Values)
                extension.OnSetup();
        }

        private void DisposeExtensions()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.OnDispose();
            _extensionInstances.Clear();
        }

        private void NotifyExtensionsOnContextStarting()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.OnContextStarting();
        }
        
        private void NotifyExtensionsOnContextStarted()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.OnContextStarted();
        }

        private void NotifyExtensionsAboutContextStopping()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.OnContextStopping();
        }
        
        private void NotifyExtensionsAboutContextStopped()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.OnContextStopped();
        }

        /*
         * Modules.
         */

        public IContext AddModule<T>() where T : IModule, new()
        {
            if (_modules.Contains(typeof(T)))
                throw new ContextException(ContextExceptionType.ModuleAlreadyAdded, typeof(T).FullName);
            _modules.Add(typeof(T));
            return this;
        }

        private void InitializeModules()
        {
            // This must be done in for loop as modules list might change during loop execution.
            for (var i = 0; i < _modules.Count; i++)
            {
                var moduleType = _modules[i];
                var module = (IModule)Activator.CreateInstance(moduleType);
                module.SetContext(this);

                foreach (var extension in _extensionInstances.Values)
                    extension.OnModuleConstructed(module);

                module.Configure();
                _moduleInstances.Add(moduleType, module);
            }
        }

        private void DisposeModules()
        {
            foreach (var module in _moduleInstances.Values)
            foreach (var extension in _extensionInstances.Values)
                extension.OnModuleDispose(module);
            _moduleInstances.Clear();
        }

        /*
         * Start / Stop.
         */

        public IContext Start()
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ContextAlreadyStarted);

            InitializeExtensions();
            InitializeModules();
            
            NotifyExtensionsOnContextStarting();
            
            OnStart?.Invoke();
            OnContextStart?.Invoke(this);
            
            NotifyExtensionsOnContextStarted();
            
            IsStarted = true;
            return this;
        }

        public void Stop()
        {
            if (IsStopping)
                return;
            
            if (!IsStarted)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            IsStopping = true;
            
            NotifyExtensionsAboutContextStopping();
            
            OnStop?.Invoke();
            OnContextStop?.Invoke(this);
            
            NotifyExtensionsAboutContextStopped();
            
            DisposeModules();
            DisposeExtensions();

            IsStopping = false;
            IsStarted = false;
        }
    }
}