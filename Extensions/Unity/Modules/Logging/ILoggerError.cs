using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerError
    {
        void Error(string message);
        void Error(Exception exception);
    }
}