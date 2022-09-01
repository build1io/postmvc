using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.Contexts.Impl;
using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Modules;

namespace Build1.PostMVC.Core.Contexts
{
    internal sealed class Context : IContext
    {
        public bool IsRootContext { get; }
        public bool IsStarted     { get; private set; }
        public bool IsQuitting    { get; private set; }
        public bool IsStopping    { get; private set; }

        public event Action<Module> OnModuleConstructing;
        public event Action<Module> OnModuleDisposing;

        public event Action OnStarting;
        public event Action OnStarted;
        public event Action OnQuitting;
        public event Action OnStopping;
        public event Action OnStopped;

        private readonly IContext _rootContext;
        
        private readonly HashSet<Type>                _extensions;
        private readonly Dictionary<Type, Extension> _extensionInstances;

        private readonly List<Type>                _modules;
        private readonly Dictionary<Type, Module> _moduleInstances;

        public Context(IContext rootContext)
        {
            _rootContext = rootContext ?? this;
            
            _extensions = new HashSet<Type>();
            _extensionInstances = new Dictionary<Type, Extension>();

            _modules = new List<Type>();
            _moduleInstances = new Dictionary<Type, Module>();

            IsRootContext = rootContext == this;
        }

        /*
         * Extensions.
         */

        public IContext AddExtension(Extension extension)
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

        public IContext AddExtension<T>() where T : Extension, new()
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ExtensionInstallationAttemptAfterContextStart, typeof(T).FullName);

            _extensions.Add(typeof(T));
            return this;
        }

        public bool HasExtension<T>() where T : Extension
        {
            return _extensions.Contains(typeof(T));
        }

        public T GetExtension<T>() where T : Extension
        {
            if (_extensionInstances.TryGetValue(typeof(T), out var extension))
                return (T)extension;
            throw new ContextException(ContextExceptionType.ExtensionInstanceNotFound);
        }

        public bool TryGetExtension<T>(out T extension) where T : Extension
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
                    extension = (Extension)Activator.CreateInstance(extensionType);
                    _extensionInstances.Add(extensionType, extension);
                }

                extension.SetContext(this, _rootContext);
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

        public IContext AddModule<T>() where T : Module, new()
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
                var module = (Module)Activator.CreateInstance(moduleType);
                module.SetContext(this);

                OnModuleConstructing?.Invoke(module);

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
            PostMVC.OnContextStartingHandler(this);

            IsStarted = true;

            OnStarted?.Invoke();
            PostMVC.OnContextStartedHandler(this);
        }

        public void SetQuitting()
        {
            if (IsQuitting)
                return;

            if (!IsStarted)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            IsQuitting = true;
            
            OnQuitting?.Invoke();
            PostMVC.OnContextQuittingHandler(this);
        }

        public void Stop()
        {
            if (IsStopping || !IsStarted)
                return;

            IsStopping = true;

            OnStopping?.Invoke();
            PostMVC.OnContextStoppingHandler(this);

            IsStopping = false;
            IsStarted = false;

            OnStopped?.Invoke();
            PostMVC.OnContextStoppedHandler(this);

            DisposeModules();
            DisposeExtensions();
        }
    }
}