using System;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    [Flags]
    public enum ReflectionParams
    {
        // Will make framework to prepare reflection info for all registered bindings at the moment of context start. 
        PrepareBindingsReflectionInfoOnContextStart = 1 << 0,
        
        // Will make framework to prepare reflection info for all sealed views and/or mediators defined in the app.
        PrepareViewsReflectionInfoOnContextStart     = 1 << 1,
        PrepareMediatorsReflectionInfoOnContextStart = 1 << 2,
        
        // Will make framework to prepare reflection info for all commands at the moment of context start.
        PrepareCommandsReflectionInfoOnContextStart = 1 << 3,
        
        // Prepares all reflection info on context start.
        PrepareReflectionInfoOnContextStart = PrepareBindingsReflectionInfoOnContextStart |
                                              PrepareCommandsReflectionInfoOnContextStart |
                                              PrepareViewsReflectionInfoOnContextStart | 
                                              PrepareMediatorsReflectionInfoOnContextStart
    }
}