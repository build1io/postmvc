using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    [Flags]
    public enum LogLevel
    {
        None    = 0,
        Debug   = 1,
        Warning = 2,
        Error   = 4,

        All        = Error | Warning | Debug,
        Verbose    = Error | Warning,
        ErrorsOnly = Error
    }
}