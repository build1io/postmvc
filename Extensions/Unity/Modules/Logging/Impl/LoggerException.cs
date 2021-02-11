using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LoggerException : Exception
    {
        public LoggerException(LoggerExceptionType type) : base(type.ToString())
        {
        }
        
        public LoggerException(LoggerExceptionType type, string message) : base($"{type} [{message}]")
        {
        }
    }
}