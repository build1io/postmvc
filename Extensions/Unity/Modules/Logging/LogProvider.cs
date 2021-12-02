using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LogProvider : InjectionProvider<Log, ILog>
    {
        public static LogLevel GlobalLogLevelOverride  = LogLevel.None;
        public static bool     ForceLogsInReleaseBuild = false;

        private readonly Stack<ILog> _availableInstances;
        private readonly List<ILog>  _usedInstances;

        public LogProvider()
        {
            _availableInstances = new Stack<ILog>();
            _usedInstances = new List<ILog>();
        }

        /*
         * Public.
         */

        public override ILog TakeInstance(object parent, Log attribute)
        {
            ILog log;

            if (_availableInstances.Count > 0)
            {
                log = _availableInstances.Pop();
                log.SetPrefix(parent.GetType().Name);
                log.SetLevel(GlobalLogLevelOverride != LogLevel.None ? GlobalLogLevelOverride : attribute.logLevel);
                _usedInstances.Add(log);
            }
            else
            {
                log = GetLog(parent.GetType(), attribute.logLevel);
                _usedInstances.Add(log);
            }

            return log;
        }

        public override void ReturnInstance(ILog instance)
        {
            if (_usedInstances.Remove(instance))
                _availableInstances.Push(instance);
        }

        /*
         * Static.
         */

        public static ILog GetLog<T>(LogLevel level)
        {
            return GetLog(typeof(T).Name, level);
        }

        public static ILog GetLog(Type type, LogLevel level)
        {
            return GetLog(type.Name, level);
        }

        public static ILog GetLog(string prefix, LogLevel level)
        {
            if (GlobalLogLevelOverride != LogLevel.None)
                level = GlobalLogLevelOverride;

            #if UNITY_WEBGL && !UNITY_EDITOR
            
            return new LogWebGL(prefix, level);

            #elif UNITY_EDITOR

            return new LogDebug(prefix, level);

            #else
            
            // Always returns true in Editor.
            if (UnityEngine.Debug.isDebugBuild || ForceLogsInReleaseBuild)
                return new LogDebug(prefix, level);

            return new LogVoid(prefix, level);

            #endif
        }
    }
}