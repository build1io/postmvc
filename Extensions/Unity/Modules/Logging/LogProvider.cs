using System;
using System.Collections.Generic;
using System.Text;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LogProvider : InjectionProvider<LogAttribute, ILog>
    {
        public static bool ForceAllLogs   = false;
        public static bool RecordLogs     = false;
        public static bool PrintLogs      = false;
        public static uint FlushThreshold = 256;

        private static readonly StringBuilder _records = new();
        private static          int           _recordsCount;

        public static event Action<string> OnFlush;

        private readonly Stack<ILog> _availableInstances;
        private readonly List<ILog>  _usedInstances;

        public LogProvider()
        {
            _availableInstances = new Stack<ILog>();
            _usedInstances = new List<ILog>();
        }

        /*
         * Instances.
         */

        public override ILog TakeInstance(object parent, LogAttribute attribute)
        {
            ILog log;

            if (_availableInstances.Count > 0)
            {
                log = _availableInstances.Pop();
                log.SetPrefix(parent.GetType().Name);
                log.SetLevel(ForceAllLogs ? LogLevel.All : attribute.logLevel);
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
         * Loggers creation.
         */

        public static ILog GetLog<T>(LogLevel level)
        {
            var log = GetImpl(typeof(T).Name, level);

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
            return GetImpl(owner.GetType().Name, level);
        }

        private static ILog GetImpl(string prefix, LogLevel level)
        {
            if (ForceAllLogs)
                level = LogLevel.All;

            // Debug.isDebugBuild is always true in Editor.
            if (Debug.isDebugBuild || ForceAllLogs || PrintLogs || RecordLogs)
            {
                #if UNITY_WEBGL && !UNITY_EDITOR
                return new LogWebGL(prefix, level, Debug.isDebugBuild || PrintLogs, RecordLogs);

                #else

                return new LogDefault(prefix, level, Debug.isDebugBuild || PrintLogs, RecordLogs);

                #endif
            }

            return new LogVoid(prefix, level);
        }

        /*
         * Log Records.
         */

        internal static void RecordMessage(string message)
        {
            _records.AppendLine(message);
            _recordsCount++;

            if (_recordsCount >= FlushThreshold)
                FlushLogs();
        }

        public static string FlushLogs()
        {
            var logs = _records.ToString();

            _records.Clear();
            _recordsCount = 0;

            OnFlush?.Invoke(logs);

            return logs;
        }
    }
}