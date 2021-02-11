using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.MVCS.Context
{
    public abstract class ContextEvent
    {
        public static readonly Event Started = new Event();
        public static readonly Event Stopped = new Event();
    }
}