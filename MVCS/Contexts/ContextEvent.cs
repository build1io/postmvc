using Build1.PostMVC.Core.Contexts;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Contexts
{
    public abstract class ContextEvent
    {
        public static readonly Event<IContext> Started = new(typeof(ContextEvent), nameof(Started));
        public static readonly Event<IContext> Stopped = new(typeof(ContextEvent), nameof(Stopped));
    }
}