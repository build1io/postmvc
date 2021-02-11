using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerWarn
    {
        ILoggerWarn Warn(string message);
        ILoggerWarn Warn(Exception exception);
    }
}