using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.Extensions;
using Build1.PostMVC.Core.Modules;

namespace Build1.PostMVC.Core.Contexts.Impl
{
    internal sealed class Context : IContext
    {
        public int    Id            { get; }
        public string Name          { get; }
        public bool   IsRootContext { get; }
        public bool   IsStarted     { get; private set; }
        public bool   IsQuitting    { get; private set; }
        public bool   IsStopping    { get; private set; }

        public event Action<Module> OnModuleConstructing;
        public event Action<Module> OnModuleDisposing;

        public event Action<IContext> OnStarting;
        public event Action<IContext> OnStarted;
        public event Action<IContext> OnQuitting;
        public event Action<IContext> OnStopping;
        public event Action<IContext> OnStopped;

        private readonly IContext _rootContext;

        private readonly HashSet<Type>               _extensions;
        private readonly Dictionary<Type, Extension> _extensionInstances;

        private readonly List<Type>               _modules;
        private readonly Dictionary<Type, Module> _moduleInstances;

        public Context(int id, string name, IContext rootContext)
        {
            _rootContext = rootContext ?? this;

            _extensions = new HashSet<Type>();
            _extensionInstances = new Dictionary<Type, Extension>();

            _modules = new List<Type>();
            _moduleInstances = new Dictionary<Type, Module>();

            Id = id;
            Name = name;
            IsRootContext = _rootContext == this;
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

        private void InitializeModules(int extensionModulesStartIndex, int extensionModulesCount)
        {
            // Modules initialization must be done in a for loop as modules list might change during execution.
            
            for (var i = extensionModulesStartIndex; i < extensionModulesCount; i++)
                InitializeModule(_modules[i]);
            
            for (var i = 0; i < _modules.Count; i++)
            {
                if (i < extensionModulesStartIndex || i >= extensionModulesCount)
                    InitializeModule(_modules[i]);
            }
        }

        private void InitializeModule(Type moduleType)
        {
            var module = (Module)Activator.CreateInstance(moduleType);
            module.SetContext(this);

            OnModuleConstructing?.Invoke(module);

            _moduleInstances.Add(moduleType, module);
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

        public IContext Start()
        {
            if (IsStarted)
                throw new ContextException(ContextExceptionType.ContextAlreadyStarted);

            var startModulesCount = _modules.Count;
            
            InitializeExtensions();

            var extensionsModulesStartIndex = startModulesCount;
            var extensionsModulesCount = _modules.Count - startModulesCount; 
            
            InitializeModules(extensionsModulesStartIndex, extensionsModulesCount);

            OnStarting?.Invoke(this);
            PostMVC.OnContextStartingHandler(this);

            IsStarted = true;

            OnStarted?.Invoke(this);
            PostMVC.OnContextStartedHandler(this);

            return this;
        }

        public void SetQuitting()
        {
            if (IsQuitting)
                return;

            if (!IsStarted)
                throw new ContextException(ContextExceptionType.ContextNotStarted);

            IsQuitting = true;

            OnQuitting?.Invoke(this);
            PostMVC.OnContextQuittingHandler(this);
        }

        public void Stop()
        {
            if (IsStopping || !IsStarted)
                return;

            IsStopping = true;

            OnStopping?.Invoke(this);
            PostMVC.OnContextStoppingHandler(this);

            IsStopping = false;
            IsStarted = false;

            OnStopped?.Invoke(this);
            PostMVC.OnContextStoppedHandler(this);

            DisposeModules();
            DisposeExtensions();
        }
    }
}