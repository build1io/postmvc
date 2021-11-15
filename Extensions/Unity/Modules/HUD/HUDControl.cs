using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.HUD
{
    public sealed class HUDControl : UIControl<HUDControlConfig>
    {
        public HUDControl(string name) : base(name, UIControlBehavior.Default)
        {
        }
        
        public HUDControl(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
    }
}