using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging
{
    public interface ILogger
    {
        /*
         * Debug.
         */
        
        ILoggerDebug Debug(string message);
        ILoggerDebug Debug(Exception exception);
        
        ILoggerDebug Debug(Func<string> callback);
        ILoggerDebug Debug<T1>(Func<T1, string> callback, T1 param01);
        ILoggerDebug Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        ILoggerDebug Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        ILoggerDebug Debug(Action<ILoggerDebug> callback);
        ILoggerDebug Debug<T1>(Action<ILoggerDebug, T1> callback, T1 param01);
        ILoggerDebug Debug<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02);
        ILoggerDebug Debug<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Warn.
         */
        
        ILoggerWarn Warn(string message);
        ILoggerWarn Warn(Exception exception);
        
        ILoggerWarn Warn(Func<string> callback);
        ILoggerWarn Warn<T1>(Func<T1, string> callback, T1 param01);
        ILoggerWarn Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        ILoggerWarn Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        ILoggerWarn Warn(Action<ILoggerWarn> callback);
        ILoggerWarn Warn<T1>(Action<ILoggerWarn, T1> callback, T1 param01);
        ILoggerWarn Warn<T1, T2>(Action<ILoggerWarn, T1, T2> callback, T1 param01, T2 param02);
        ILoggerWarn Warn<T1, T2, T3>(Action<ILoggerWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Error.
         */
        
        ILoggerError Error(string message);
        ILoggerError Error(Exception exception);
        
        ILoggerError Error(Func<string> callback);
        ILoggerError Error<T1>(Func<T1, string> callback, T1 param01);
        ILoggerError Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        ILoggerError Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        ILoggerError Error(Action<ILoggerError> callback);
        ILoggerError Error<T1>(Action<ILoggerError, T1> callback, T1 param01);
        ILoggerError Error<T1, T2>(Action<ILoggerError, T1, T2> callback, T1 param01, T2 param02);
        ILoggerError Error<T1, T2, T3>(Action<ILoggerError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Instance.
         */
        
        ILogger SetLevel(LogLevel level);
        ILogger Disable();
    }
}