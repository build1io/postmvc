using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    [Flags]
    public enum UIControlBehavior
    {
        PreInstantiate = 1 << 0,
        DestroyOnClose = 1 << 1
    }
}