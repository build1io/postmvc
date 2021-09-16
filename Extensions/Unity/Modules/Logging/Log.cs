using System;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class Log : Inject
    {
        public readonly LogLevel logLevel;

        public Log(LogLevel logLevel)
        {
            this.logLevel = logLevel;
        }
    }
}