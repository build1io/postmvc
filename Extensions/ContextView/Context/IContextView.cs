using Build1.PostMVC.Contexts;

namespace Build1.PostMVC.Extensions.ContextView.Context
{
    public interface IContextView
    {
        IContext Context { get; }
        object   ViewRaw { get; }

        T As<T>() where T : class;
    }
}