using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.FullScreen
{
    public static class FullScreenEvent
    {
        public static readonly Event<bool> Changed = new Event<bool>();
    }
}