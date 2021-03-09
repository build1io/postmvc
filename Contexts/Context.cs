using System;
using System.Collections.Generic;
using Build1.PostMVC.Contexts.Impl;
using Build1.PostMVC.Extensions;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Contexts
{
    public sealed class Context : IContext
    {
        public static event Action<IContext> OnContextStarting;
        public static event Action<IContext> OnContextStarted;
        public static event Action<IContext> OnContextQuitting;
        public static event Action<IContext> OnContextStopping;
        public static event Action<IContext> OnContextStopped;

        public IContext RootContext { get; }

        public bool IsRootContext => this == RootContext;
        public bool IsStarted     { get; private set; }
        public bool IsQuitting    { get; private set; }
        public bool IsStopping    { get; private set; }

        public event Action<IModule> OnModuleConstructing;
        public event Action<IModule> OnModuleDisposing;

        public event Action OnStarting;
        public event Action OnStarted;
        public event Action OnQuitting;
        public event Action OnStopping;
        public event Action OnStopped;

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

                extension.SetContext(this, RootContext);
            }

            foreach (var extension in _extensionInstances.Values)
                extension.Initialize();

            foreach (var extension in _extensionInstances.Values)
                extension.Setup();
        }

        private void DisposeExtensions()
        {
            foreach (var extension in _extensionInstances.Values)
                extension.Dispose();
            _extensionInstances.Clear();
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

                OnModuleConstructing?.Invoke(module);

                module.Configure();
                _moduleInstances.Add(moduleType, module);
            }
        }

        private void DisposeModules()
        {
            foreach (var module in _moduleInstances.Values)
                OnModuleDisposing?.Invoke(module);
            _moduleInstances.Clear();
        }

        /*
         * Start / Stop.
         */

        public void Start()
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ContextAlreadyStarted);

            InitializeExtensions();
            InitializeModules();

            OnStarting?.Invoke();
            OnContextStarting?.Invoke(this);

            IsStarted = true;

            OnStarted?.Invoke();
            OnContextStarted?.Invoke(this);
        }

        public void SetQuitting()
        {
            if (IsQuitting)
                return;

            if (!IsStarted)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            IsQuitting = true;
            
            OnQuitting?.Invoke();
            OnContextQuitting?.Invoke(this);
        }

        public void Stop()
        {
            if (IsStopping)
                return;

            if (!IsStarted)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            IsStopping = true;

            OnStopping?.Invoke();
            OnContextStopping?.Invoke(this);

            IsStopping = false;
            IsStarted = false;

            OnStopped?.Invoke();
            OnContextStopped?.Invoke(this);

            DisposeModules();
            DisposeExtensions();
        }
    }
}