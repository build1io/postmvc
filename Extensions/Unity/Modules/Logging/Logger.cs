using System;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Logger : Inject
    {
        public readonly LogLevel logLevel;

        public Logger(LogLevel logLevel)
        {
            this.logLevel = logLevel;
        }
    }
}