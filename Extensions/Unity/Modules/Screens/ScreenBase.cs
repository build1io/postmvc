using System;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public abstract class ScreenBase : UIControl<ScreenConfig>
    {
        public readonly Type dataType;
        
        protected ScreenBase(string name, UIControlBehavior behavior) : base(name, behavior)
        {
        }
        
        protected ScreenBase(string name, UIControlBehavior behavior, Type dataType) : base(name, behavior)
        {
            this.dataType = dataType;
        }
    }
}