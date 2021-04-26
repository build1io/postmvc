using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    [Flags]
    public enum LogLevel
    {
        None    = 0,
        All     = 1,
        
        Debug   = 10,
        Warning = 20,
        Error   = 30
    }
}