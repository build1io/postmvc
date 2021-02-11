using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerDebug
    {
        ILoggerDebug Debug(string message);
        ILoggerDebug Debug(Exception exception);
    }
}