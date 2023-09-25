using System;

namespace Build1.PostMVC.Core.MVCS.Injection
{
    [Flags]
    public enum InjectionParams
    {
        PrepareReflectionDataOnContextStart = 1 << 0
    }
}