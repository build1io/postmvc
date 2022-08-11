using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public sealed class Screen : ScreenBase
    {
        public Screen(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
    
    public sealed class Screen<T> : ScreenBase
    {
        public Screen(string name, UIControlBehavior behavior) : base(name, behavior, typeof(T))
        {
        }
    }
}