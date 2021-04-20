using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LoggerProvider : InjectionProvider<Logger, ILogger>
    {
        public static LogLevel GlobalLogLevelOverride = LogLevel.None;

        private readonly Stack<ILogger> _availableInstances;
        private readonly List<ILogger>  _usedInstances;

        public LoggerProvider()
        {
            _availableInstances = new Stack<ILogger>();
            _usedInstances = new List<ILogger>();
        }

        /*
         * Public.
         */

        public override ILogger TakeInstance(object parent, Logger attribute)
        {
            ILogger logger;

            if (_availableInstances.Count > 0)
            {
                logger = _availableInstances.Pop();
                logger.SetPrefix(parent.GetType().Name);
                logger.SetLevel(GlobalLogLevelOverride != LogLevel.None ? GlobalLogLevelOverride : attribute.logLevel);
                _usedInstances.Add(logger);
            }
            else
            {
                logger = GetLogger(parent.GetType(), attribute.logLevel);
                _usedInstances.Add(logger);
            }

            return logger;
        }

        public override void ReturnInstance(ILogger instance)
        {
            if (_usedInstances.Remove(instance))
                _availableInstances.Push(instance);
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
            if (GlobalLogLevelOverride != LogLevel.None)
                level = GlobalLogLevelOverride;

            #if UNITY_WEBGL && !UNITY_EDITOR
            
            return new LoggerWebGL(prefix, level);

            #else

            // Always returns true in Editor. 
            if (UnityEngine.Debug.isDebugBuild)
                return new LoggerDebug(prefix, level);

            return new LoggerVoid(prefix, level);

            #endif
        }
    }
}