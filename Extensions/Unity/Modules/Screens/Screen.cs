using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public class Screen : UIControl<ScreenConfig>
    {
        public Screen(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
}