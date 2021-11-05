using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    [Flags]
    public enum UIControlBehavior
    {
        /// Control will not be pre instantiated. 
        /// Control will not be destroyed on deactivation.
        /// Multiple instances of the control will be created with every invocation.
        Default = 1 << 0,
        
        /// Control will be pre instantiated on initialization.
        /// UIControlsController.Initialize method must be called. 
        PreInstantiate = 1 << 1,
        
        /// Control will be destroyed on deactivation.
        DestroyOnDeactivation = 1 << 2,
        
        /// Only one instance if this screen will be created and used. 
        SingleInstance = 1 << 3
    }
}