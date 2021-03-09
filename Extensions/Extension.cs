using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Extensions
{
    public abstract class Extension : IExtension
    {
        protected IContext Context     { get; private set; }
        protected IContext RootContext { get; private set; }

        public void SetContext(IContext context, IContext rootContext)
        {
            Context = context;
            RootContext = rootContext;
        }

        public virtual void Initialize() { }
        public virtual void Setup()      { }
        public virtual void Dispose()    { }

        protected T GetDependentExtension<T>() where T : IExtension
        {
            if (Context.TryGetExtension<T>(out var extension))
                return extension;
            throw new ExtensionException(ExtensionExceptionType.DependentExtensionNotFound, typeof(T).FullName);
        }
    }
}