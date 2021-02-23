using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    [Flags]
    public enum UIControlOptions
    {
        /// Control will be instantiated if there is no instance on the layer.
        Instantiate = 1 << 0,

        /// Defines whether control must be activate. If not control will be inactive even if it was instantiated.
        Activate = 1 << 1
    }
}