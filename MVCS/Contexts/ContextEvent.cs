using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Contexts
{
    public abstract class ContextEvent
    {
        public static readonly Event Started = new();
        public static readonly Event Stopped = new();
    }
}