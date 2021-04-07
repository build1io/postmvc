using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public static class ScreenEvent
    {
        public static readonly Event<Screen> Open   = new Event<Screen>();
        public static readonly Event<Screen> Closed = new Event<Screen>();
    }
}