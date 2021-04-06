using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.App
{
    public abstract class AppEvent
    {
        public static readonly Event<bool> Pause = new Event<bool>();
        public static readonly Event<bool> Focus = new Event<bool>();

        public static readonly Event Restarting = new Event();
        public static readonly Event Quitting   = new Event();
    }
}