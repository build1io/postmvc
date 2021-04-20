using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILogger
    {
        /*
         * Debug.
         */
        
        void Debug(string message);
        void Debug(Exception exception);
        
        void Debug(Func<string> callback);
        void Debug<T1>(Func<T1, string> callback, T1 param01);
        void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        void Debug(Action<ILoggerDebug> callback);
        void Debug<T1>(Action<ILoggerDebug, T1> callback, T1 param01);
        void Debug<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02);
        void Debug<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Warn.
         */
        
        void Warn(string message);
        void Warn(Exception exception);
        
        void Warn(Func<string> callback);
        void Warn<T1>(Func<T1, string> callback, T1 param01);
        void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        void Warn(Action<ILoggerWarn> callback);
        void Warn<T1>(Action<ILoggerWarn, T1> callback, T1 param01);
        void Warn<T1, T2>(Action<ILoggerWarn, T1, T2> callback, T1 param01, T2 param02);
        void Warn<T1, T2, T3>(Action<ILoggerWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Error.
         */
        
        void Error(string message);
        void Error(Exception exception);
        
        void Error(Func<string> callback);
        void Error<T1>(Func<T1, string> callback, T1 param01);
        void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        void Error(Action<ILoggerError> callback);
        void Error<T1>(Action<ILoggerError, T1> callback, T1 param01);
        void Error<T1, T2>(Action<ILoggerError, T1, T2> callback, T1 param01, T2 param02);
        void Error<T1, T2, T3>(Action<ILoggerError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Instance.
         */

        void SetPrefix(string prefix);
        void SetLevel(LogLevel level);
        void Disable();
    }
}