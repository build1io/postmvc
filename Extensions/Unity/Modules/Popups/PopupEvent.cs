using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popups
{
    public static class PopupEvent
    {
        public static readonly Event<PopupBase> Open   = new Event<PopupBase>();
        public static readonly Event<PopupBase> Closed = new Event<PopupBase>();
    }
}