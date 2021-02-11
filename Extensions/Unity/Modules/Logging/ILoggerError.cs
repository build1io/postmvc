using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILoggerError
    {
        ILoggerError Error(string message);
        ILoggerError Error(Exception exception);
    }
}