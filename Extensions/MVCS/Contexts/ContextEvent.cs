using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.Core.Extensions.MVCS.Contexts
{
    public abstract class ContextEvent
    {
        public static readonly Event Started = new Event();
        public static readonly Event Stopped = new Event();
    }
}