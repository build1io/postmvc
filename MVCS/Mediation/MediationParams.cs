using System;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    [Flags]
    public enum MediationParams
    {
        // This will make framework throw exception if mediator binding is not defined for a view.
        StrictMediation = 1 << 0
    }
}