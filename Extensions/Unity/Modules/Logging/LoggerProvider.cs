using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LoggerProvider : InjectionProvider<Logger>
    {
        public override object GetInstance(object parent, Logger attribute)
        {
            return GetLogger(parent.GetType(), attribute.logLevel);
        }

        public override void ReturnInstance(object instance)
        {
        }

        /*
         * Static.
         */
        
        public static ILogger GetLogger<T>(LogLevel level)
        {
            return GetLogger(typeof(T).Name, level);
        }

        public static ILogger GetLogger(Type type, LogLevel level)
        {
            return GetLogger(type.Name, level);
        }

        public static ILogger GetLogger(string prefix, LogLevel level)
        {
            #if UNITY_WEBGL && !UNITY_EDITOR
                return new LoggerWebGL(prefix, level);
            #else
                return new LoggerDebug(prefix, level);
            #endif
        }
    }
}