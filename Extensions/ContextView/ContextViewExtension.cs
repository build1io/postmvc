using Build1.PostMVC.Extensions.ContextView.Contexts;
using Build1.PostMVC.Extensions.MVCS;

namespace Build1.PostMVC.Extensions.ContextView
{
    public sealed class ContextViewExtension : Extension
    {
        public object View { get; }

        public ContextViewExtension()
        {
        }

        public ContextViewExtension(object view)
        {
            View = view;
        }

        public override void Initialize()
        {
            var injectionBinder = GetDependentExtension<MVCSExtension>().InjectionBinder;
            injectionBinder.Bind<IContextView>().ToValue(new Contexts.Impl.ContextView(View, Context));
        }

        public override void Dispose()
        {
            // We don't need to unbind anything. MVCSExtension does it.
        }
    }
}