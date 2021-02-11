using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public static class ScreenEvent
    {
        public static readonly Event<Screen> Open   = new Event<Screen>();
        public static readonly Event<Screen> Closed = new Event<Screen>();
    }
}