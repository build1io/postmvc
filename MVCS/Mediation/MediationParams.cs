using System;

namespace Build1.PostMVC.Core.MVCS.Mediation
{
    [Flags]
    public enum MediationParams
    {
        // This will make framework throw exception if mediator binding is not defined for a view.
        StrictMediation = 1 << 0,

        // These will make framework collect prepare reflection data for all sealed views and/or mediators defined in the app.
        PrepareViewsReflectionInfoOnContextStart     = 1 << 1,
        PrepareMediatorsReflectionInfoOnContextStart = 1 << 2
    }
}