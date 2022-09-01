using Build1.PostMVC.Core.Contexts;

namespace Build1.PostMVC.Core.Extensions
{
    public abstract class Extension
    {
        protected IContext Context     { get; private set; }
        protected IContext RootContext { get; private set; }

        internal void SetContext(IContext context, IContext rootContext)
        {
            Context = context;
            RootContext = rootContext;
        }

        public virtual void Initialize() { }
        public virtual void Setup()      { }
        public virtual void Dispose()    { }

        protected T GetDependentExtension<T>() where T : Extension
        {
            if (Context.TryGetExtension<T>(out var extension))
                return extension;
            throw new ExtensionException(ExtensionExceptionType.DependentExtensionNotFound, typeof(T).FullName);
        }
    }
}