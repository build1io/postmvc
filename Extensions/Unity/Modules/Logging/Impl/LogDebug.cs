using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LogDebug : LogBase
    {
        public LogDebug(string prefix, LogLevel level) : base(prefix, level)
        {
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
        
        private static void DebugImpl(string message)
        {
            UnityEngine.Debug.Log(message);
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
        
        private static void WarningImpl(string message)
        {
            UnityEngine.Debug.LogWarning(message);
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
        
        private static void ErrorImpl(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }
}