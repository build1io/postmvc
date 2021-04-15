using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    [Flags]
    public enum LogLevel
    {
        None    = 0,
        Debug   = 1 << 0,
        Warning = 1 << 1,
        Error   = 1 << 2,

        All        = Error | Warning | Debug,
        Verbose    = Error | Warning,
        ErrorsOnly = Error
    }
}