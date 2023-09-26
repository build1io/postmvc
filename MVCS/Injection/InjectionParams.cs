using System;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    [Flags]
    public enum InjectionParams
    {
        PrepareReflectionInfoOnContextStart = 1 << 0
    }
}