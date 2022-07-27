using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LogDefault : LogBase
    {
        public LogDefault(string prefix, LogLevel level, bool print, bool record) : base(prefix, level, print, record)
        {
        }
        
        /*
         * Debug.
         */
        
        public override void Debug(string message)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            message = FormatMessage(message);
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Debug(Exception exception)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatException(exception);
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Debug(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Debug<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01));
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Debug))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (_print)
                UnityEngine.Debug.Log(message);
            
            if (_record)
                RecordMessage(message);
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
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Warn(Exception exception)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatException(exception);
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Warn(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Warn<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01));
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Warning))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (_print)
                UnityEngine.Debug.LogWarning(message);
            
            if (_record)
                RecordMessage(message);
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
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Error(Exception exception)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatException(exception);
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Error(Func<string> callback)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke());
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Error<T1>(Func<T1, string> callback, T1 param01)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01));
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02));
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
        }

        public override void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if (!CheckLevel(LogLevel.Error))
                return;

            var message = FormatMessage(callback.Invoke(param01, param02, param03));
            
            if (_print)
                UnityEngine.Debug.LogError(message);
            
            if (_record)
                RecordMessage(message);
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