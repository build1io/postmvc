using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    [Flags]
    public enum UIControlBehavior
    {
        /// Control will be pre instantiated on initialization.
        /// UIControlsController.Initialize method must be called. 
        PreInstantiate = 1 << 0,
        
        /// Control will be destroyed on deactivation.
        DestroyOnDeactivation = 1 << 1
    }
}