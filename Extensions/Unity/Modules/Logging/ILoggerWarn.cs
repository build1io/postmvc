using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerWarn
    {
        void Warn(string message);
        void Warn(Exception exception);
    }
}