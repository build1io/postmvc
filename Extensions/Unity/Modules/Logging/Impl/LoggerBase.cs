using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal abstract class LoggerBase : ILogger, ILoggerDebug, ILoggerWarn, ILoggerError 
    {
        private readonly string   _prefix;
        private          LogLevel _level;

        protected LoggerBase(string prefix, LogLevel mode)
        {
            _prefix = prefix;
            _level = mode;
        }

        /*
         * Debug.
         */

        public abstract void Debug(string message);
        public abstract void Debug(Exception exception);
        public abstract void Debug(Func<string> callback);
        public abstract void Debug<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Debug(Action<ILoggerDebug> callback);
        public abstract void Debug<T1>(Action<ILoggerDebug, T1> callback, T1 param01);
        public abstract void Debug<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Debug<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Warn.
         */

        public abstract void Warn(string message);
        public abstract void Warn(Exception exception);
        public abstract void Warn(Func<string> callback);
        public abstract void Warn<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Warn(Action<ILoggerWarn> callback);
        public abstract void Warn<T1>(Action<ILoggerWarn, T1> callback, T1 param01);
        public abstract void Warn<T1, T2>(Action<ILoggerWarn, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Warn<T1, T2, T3>(Action<ILoggerWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Error.
         */

        public abstract void Error(string message);
        public abstract void Error(Exception exception);
        public abstract void Error(Func<string> callback);
        public abstract void Error<T1>(Func<T1, string> callback, T1 param01);
        public abstract void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        public abstract void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        public abstract void Error(Action<ILoggerError> callback);
        public abstract void Error<T1>(Action<ILoggerError, T1> callback, T1 param01);
        public abstract void Error<T1, T2>(Action<ILoggerError, T1, T2> callback, T1 param01, T2 param02);
        public abstract void Error<T1, T2, T3>(Action<ILoggerError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Public.
         */

        public void SetLevel(LogLevel level)
        {
            _level = level;
        }

        public void Disable()
        {
            SetLevel(LogLevel.None);
        }

        /*
         * Protected.
         */
        
        protected bool   CheckLevel(LogLevel level)           { return (_level & level) == level; }
        protected string FormatMessage(object message)        { return FormatMessage(_prefix, message); }
        protected string FormatException(Exception exception) { return FormatException(_prefix, exception); }
        
        /*
         * Static.
         */
        
        protected static string FormatMessage(string prefix, object message)
        {
            return $"{prefix}: {message}";
        }

        protected static string FormatException(string prefix, Exception exception)
        {
            return $"{prefix}: {exception.GetType().Name}: {exception.Message}";
        }
    }
}