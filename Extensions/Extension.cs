using Build1.PostMVC.Contexts;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions
{
    public abstract class Extension : IExtension
    {
        public IContext Context     { get; private set; }
        public IContext RootContext { get; private set; }

        public void SetContext(IContext context)
        {
            Context = context;
        }

        public void SetRootContext(IContext rootContext)
        {
            RootContext = rootContext;
        }

        public virtual void OnInitialized() { }
        public virtual void OnSetup()       { }
        public virtual void OnDispose()     { }

        public virtual void OnModuleConstructed(IModule module) { }
        public virtual void OnModuleDispose(IModule module)     { }

        public virtual void OnContextStarting() { }
        public virtual void OnContextStarted()  { }

        public virtual void OnContextStopping() { }
        public virtual void OnContextStopped()  { }

        protected T GetDependentExtension<T>() where T : IExtension
        {
            if (Context.TryGetExtension<T>(out var extension))
                return extension;
            throw new ExtensionException(ExtensionExceptionType.DependentExtensionNotFound, typeof(T).FullName);
        }
    }
}