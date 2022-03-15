using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popups
{
    public sealed class Popup : PopupBase
    {
        public Popup(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
    
    public sealed class Popup<T> : PopupBase
    {
        public Popup(string name, UIControlBehavior behavior) : base(name, behavior, typeof(T))
        {
        }
    }
}