using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Extensions.ContextView.Contexts.Impl
{
    internal sealed class ContextView : IContextView
    {
        public IContext Context { get; }
        public object   ViewRaw { get; }

        public ContextView(object view, IContext context)
        {
            ViewRaw = view;
            Context = context;
        }

        public T As<T>() where T : class
        {
            return (T)ViewRaw;
        }
    }
}