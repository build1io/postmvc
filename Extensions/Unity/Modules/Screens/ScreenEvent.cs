using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<Screen> Created   = new Event<Screen>();
        public static readonly Event<Screen> Destroyed = new Event<Screen>();

        public static readonly Event<Screen> Shown  = new Event<Screen>();
        public static readonly Event<Screen> Hidden = new Event<Screen>();
    }
}