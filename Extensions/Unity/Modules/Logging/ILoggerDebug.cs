using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerDebug
    {
        void Debug(string message);
        void Debug(Exception exception);
    }
}