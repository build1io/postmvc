using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public sealed class LoggerProvider : InjectionProvider<Logger>
    {
        public static LogLevel GlobalLogLevelOverride = LogLevel.None;
        
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
            if (GlobalLogLevelOverride != LogLevel.None)
                level = GlobalLogLevelOverride;
            
            #if UNITY_WEBGL && !UNITY_EDITOR
            
            return new LoggerWebGL(prefix, level);
            
            #else
            
            // Always returns true in Editor. 
            if (Debug.isDebugBuild)
                return new LoggerDebug(prefix, level);
            return new LoggerVoid(prefix, level);
            
            #endif
        }
    }
}