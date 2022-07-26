using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LogProvider : InjectionProvider<LogAttribute, ILog>
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

        public override ILog TakeInstance(object parent, LogAttribute attribute)
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
                log = GetLog(parent, attribute.logLevel);
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
            var log = GetLog(typeof(T).Name, level);

            if (typeof(MonoBehaviour).IsAssignableFrom(typeof(T)))
            {
                log.Warn("You're getting a logger during MonoBehavior instantiation. " +
                         "This may end up in script instantiation exception on a device. " +
                         "Consider inheriting of component from UnityView and injecting a logger.");    
            }
            
            return log;
        }

        public static ILog GetLog(object owner, LogLevel level)
        {
            return GetLog(owner.GetType().Name, level);
        }

        private static ILog GetLog(string prefix, LogLevel level)
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