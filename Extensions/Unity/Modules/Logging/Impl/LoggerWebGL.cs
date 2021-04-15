#if UNITY_WEBGL

using System;
using System.Collections.Specialized;
using System.Web;
using System.Runtime.InteropServices;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LoggerWebGL : LoggerBase
    {
        [DllImport("__Internal")]
        private static extern void LogDebug(string message);

        [DllImport("__Internal")]
        private static extern void LogWarning(string message);

        [DllImport("__Internal")]
        private static extern void LogError(string message);

        [DllImport("__Internal")]
        private static extern string GetUrlParameters();

        private static readonly NameValueCollection _urlParams;
        private static readonly LogLevel            _logLevelOverride;

        static LoggerWebGL()
        {
            _urlParams = HttpUtility.ParseQueryString(GetUrlParameters().ToLower());

            try
            {
                var logLevelString = _urlParams["loglevel"];
                if (string.IsNullOrWhiteSpace(logLevelString))
                    return;
                
                var logLevel = (LogLevel)Enum.Parse(typeof(LogLevel), logLevelString, true);
                if (!Enum.IsDefined(typeof(LogLevel), logLevel))
                    return;
                
                _logLevelOverride = logLevel;
                LogDebug(FormatMessage("LoggerWebGL", $"Global log level overridden to {_logLevelOverride}"));
            }
            catch (Exception exception)
            {
                LogError(FormatException("LoggerWebGL", exception));
            }
        }

        public LoggerWebGL(string prefix, LogLevel level) : base(prefix, ValidateLogLevel(level))
        {
        }

        private static LogLevel ValidateLogLevel(LogLevel logLevel)
        {
            return _logLevelOverride != LogLevel.None ? _logLevelOverride : logLevel;
        }

        /*
         * Debug.
         */

        public override void Debug(string message)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatMessage(message));
        }

        public override void Debug(Exception exception)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatException(exception));
        }

        public override void Debug(Func<string> callback)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatMessage(callback.Invoke()));
        }

        public override void Debug<T1>(Func<T1, string> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatMessage(callback.Invoke(param01)));
        }

        public override void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatMessage(callback.Invoke(param01, param02)));
        }

        public override void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Debug))
                DebugImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
        }

        public override void Debug(Action<ILoggerDebug> callback)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this);
        }

        public override void Debug<T1>(Action<ILoggerDebug, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01);
        }

        public override void Debug<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01, param02);
        }

        public override void Debug<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01, param02, param03);
        }

        private static void DebugImpl(string message)
        {
            LogDebug(message);
        }

        /*
         * Warning.
         */

        public override void Warn(string message)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatMessage(message));
        }

        public override void Warn(Exception exception)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatException(exception));
        }

        public override void Warn(Func<string> callback)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatMessage(callback.Invoke()));
        }

        public override void Warn<T1>(Func<T1, string> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatMessage(callback.Invoke(param01)));
        }

        public override void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatMessage(callback.Invoke(param01, param02)));
        }

        public override void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Warning))
                WarningImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
        }

        public override void Warn(Action<ILoggerWarn> callback)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this);
        }

        public override void Warn<T1>(Action<ILoggerWarn, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01);
        }

        public override void Warn<T1, T2>(Action<ILoggerWarn, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01, param02);
        }

        public override void Warn<T1, T2, T3>(Action<ILoggerWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01, param02, param03);
        }

        private static void WarningImpl(string message)
        {
            LogWarning(message);
        }

        /*
         * Error.
         */

        public override void Error(string message)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatMessage(message));
        }

        public override void Error(Exception exception)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatException(exception));
        }

        public override void Error(Func<string> callback)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatMessage(callback.Invoke()));
        }

        public override void Error<T1>(Func<T1, string> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatMessage(callback.Invoke(param01)));
        }

        public override void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatMessage(callback.Invoke(param01, param02)));
        }

        public override void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Error))
                ErrorImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
        }

        public override void Error(Action<ILoggerError> callback)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this);
        }

        public override void Error<T1>(Action<ILoggerError, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01);
        }

        public override void Error<T1, T2>(Action<ILoggerError, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01, param02);
        }

        public override void Error<T1, T2, T3>(Action<ILoggerError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01, param02, param03);
        }

        private static void ErrorImpl(string message)
        {
            LogError(message);
        }
    }
}

#endif