using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;
using Build1.PostMVC.Extensions.Unity.Utils.Path;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LogProvider : InjectionProvider<LogAttribute, ILog>
    {
        public static bool ForceAll       = false;
        public static bool Print          = false;
        public static bool Record         = false;
        public static bool SaveToFile     = false;
        public static byte FlushThreshold = 255;
        public static byte RecordsHistory = 10;

        // TODO: handle 3rd party logs

        private static readonly StringBuilder _records = new();
        private static          int           _recordsCount;
        private static readonly DateTime      _recordsDate = DateTime.UtcNow;

        public static event Action<string> OnFlush;

        private readonly Stack<ILog> _availableInstances;
        private readonly List<ILog>  _usedInstances;

        public LogProvider()
        {
            _availableInstances = new Stack<ILog>();
            _usedInstances = new List<ILog>();
        }

        /*
         * Provider.
         */

        public override ILog TakeInstance(object parent, LogAttribute attribute)
        {
            ILog log;

            if (_availableInstances.Count > 0)
            {
                log = _availableInstances.Pop();
                log.SetPrefix(parent.GetType().Name);
                log.SetLevel(ForceAll ? LogLevel.All : attribute.logLevel);
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
            if (ForceAll)
                level = LogLevel.All;

            Print = Print || Debug.isDebugBuild; 

            // Debug.isDebugBuild is always true in Editor.
            if (ForceAll || Print || Record)
            {
                #if UNITY_WEBGL && !UNITY_EDITOR
                
                return new LogWebGL(prefix, level);

                #else

                return new LogDefault(prefix, level);

                #endif
            }

            return new LogVoid();
        }

        /*
         * Log Records.
         */

        internal static void RecordMessage(string message)
        {
            _records.AppendLine(message);
            _recordsCount++;

            if (FlushThreshold > 0 && _recordsCount >= FlushThreshold)
                FlushLogs();
        }
        
        public static string GetLog()
        {
            return _recordsCount > 0 ? _records.ToString() : string.Empty;
        }

        public static void FlushLogs()
        {
            if (_recordsCount < 1)
                return;
            
            var logs = _records.ToString();

            _records.Clear();
            _recordsCount = 0;

            if (SaveToFile)
                WriteToFile(logs);

            OnFlush?.Invoke(logs);
        }

        public static List<LogFile> GetLogFiles()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return null;
            
            var paths = Directory.EnumerateFiles(folderPath, "*.log", SearchOption.TopDirectoryOnly).ToArray();
            var infos = new List<LogFile>(paths.Length);

            foreach (var path in paths)
                infos.Add(new LogFile(path));

            return infos;
        }

        public static LogFile GetLastLogFile()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return null;
            
            var directory = new DirectoryInfo(folderPath);
            var file = directory.GetFiles()
                                .OrderByDescending(f => f.LastWriteTime)
                                .First();

            return new LogFile(Path.Combine(folderPath, file.FullName));
        }

        public static void DeleteLogFiles()
        {
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return;

            var directory = new DirectoryInfo(folderPath);

            foreach (var file in directory.GetFiles())
                file.Delete();
            
            foreach (var dir in directory.GetDirectories())
                dir.Delete(true); 
        }

        /*
         * Saving to File.
         */

        private static void WriteToFile(string logs)
        {
            try
            {
                var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");

                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                var fileName = $"{_recordsDate:MM.dd.yyyy hh.mm.ss tt}.log";
                var filePath = Path.Combine(folderPath, fileName);

                if (File.Exists(filePath))
                {
                    File.AppendAllText(filePath, logs);
                }
                else
                {
                    File.WriteAllText(filePath, logs);

                    DeleteOldFiles();
                }
            }
            catch
            {
                // Ignore.
            }
        }

        private static void DeleteOldFiles()
        {
            if (RecordsHistory == 0)
                return;
            
            var folderPath = Path.Combine(PathUtil.GetPersistentDataPath(), "Logs");
            if (!Directory.Exists(folderPath))
                return;
            
            var directory = new DirectoryInfo(folderPath);
            var files = directory.GetFiles()
                                 .OrderByDescending(f => f.LastWriteTime)
                                 .ToArray();
            
            if (files.Length <= RecordsHistory)
                return;

            for (var i = RecordsHistory; i < files.Length; i++)
                files.ElementAt(i).Delete();
        }
    }
}