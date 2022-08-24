#if UNITY_WEBGL

using System;
using System.Collections.Specialized;
using System.Web;
using System.Runtime.InteropServices;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LogWebGL : LogBase
    {
        [DllImport("__Internal")]
        private static extern void LogDebug(string message);

        [DllImport("__Internal")]
        private static extern void LogWarning(string message);

        [DllImport("__Internal")]
        private static extern void LogError(string message);

        [DllImport("__Internal")]
        private static extern string GetUrlParameters();

        private static bool Print => _logLevelOverride != LogLevel.None || LogProvider.Print;
        
        private static readonly NameValueCollection _urlParams;
        private static readonly LogLevel            _logLevelOverride;

        static LogWebGL()
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
                LogDebug(FormatMessage(nameof(LogWebGL), $"Global log level overridden to {_logLevelOverride}"));
            }
            catch (Exception exception)
            {
                LogError(FormatException(nameof(LogWebGL), exception));
            }
        }

        public LogWebGL(string prefix, LogLevel level) : base(prefix, ValidateLogLevel(level))
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
            if (!CheckLevel(LogLevel.Debug))
                return;

            message = FormatMessage(message); 
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug(Exception exception)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatException(exception); 
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01)); 
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (Print)
                LogDebug(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Debug(Action<ILogDebug> callback)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this);
        }

        public override void Debug<T1>(Action<ILogDebug, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01);
        }

        public override void Debug<T1, T2>(Action<ILogDebug, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01, param02);
        }

        public override void Debug<T1, T2, T3>(Action<ILogDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Debug))
                callback.Invoke(this, param01, param02, param03);
        }

        /*
         * Warning.
         */

        public override void Warn(string message)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            message = FormatMessage(message);
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn(Exception exception)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatException(exception);
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01));
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (Print)
                LogWarning(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, false);
        }

        public override void Warn(Action<ILogWarn> callback)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this);
        }

        public override void Warn<T1>(Action<ILogWarn, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01);
        }

        public override void Warn<T1, T2>(Action<ILogWarn, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01, param02);
        }

        public override void Warn<T1, T2, T3>(Action<ILogWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Warning))
                callback.Invoke(this, param01, param02, param03);
        }

        /*
         * Error.
         */

        public override void Error(string message)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            message = FormatMessage(message);
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error(Exception exception)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatException(exception);
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01));
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (Print)
                LogError(message);
            
            if (LogProvider.Record)
                LogProvider.RecordMessage(message, true);
        }

        public override void Error(Action<ILogError> callback)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this);
        }

        public override void Error<T1>(Action<ILogError, T1> callback, T1 param01)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01);
        }

        public override void Error<T1, T2>(Action<ILogError, T1, T2> callback, T1 param01, T2 param02)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01, param02);
        }

        public override void Error<T1, T2, T3>(Action<ILogError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if (CheckLevel(LogLevel.Error))
                callback.Invoke(this, param01, param02, param03);
        }
    }
}

#endif