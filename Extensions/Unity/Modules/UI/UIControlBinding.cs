using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    public sealed class UIControlBinding
    {
        public readonly Type viewType;
        public readonly Type viewInterfaceType;
        public readonly Type mediatorType;
        
        public UIControlBinding(Type viewType)
        {
            this.viewType = viewType;
        }
        
        public UIControlBinding(Type viewType, Type mediatorType) : this(viewType)
        {
            this.mediatorType = mediatorType;
        }
        
        public UIControlBinding(Type viewType, Type viewInterfaceType, Type mediatorType) : this(viewType, mediatorType)
        {
            this.viewInterfaceType = viewInterfaceType;
        }
    }
}