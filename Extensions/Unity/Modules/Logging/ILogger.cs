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
        
        ILoggerWarn  Warn(Func<string> callback);
        ILoggerDebug Warn<T1>(Func<T1, string> callback, T1 param01);
        ILoggerDebug Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        ILoggerDebug Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        ILoggerWarn  Warn(Action<ILoggerWarn> callback);
        ILoggerDebug Warn<T1>(Action<ILoggerDebug, T1> callback, T1 param01);
        ILoggerDebug Warn<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02);
        ILoggerDebug Warn<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Error.
         */
        
        ILoggerError Error(string message);
        ILoggerError Error(Exception exception);
        
        ILoggerError Error(Func<string> callback);
        ILoggerDebug Error<T1>(Func<T1, string> callback, T1 param01);
        ILoggerDebug Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02);
        ILoggerDebug Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03);
        
        ILoggerError Error(Action<ILoggerError> callback);
        ILoggerDebug Error<T1>(Action<ILoggerDebug, T1> callback, T1 param01);
        ILoggerDebug Error<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02);
        ILoggerDebug Error<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03);

        /*
         * Instance.
         */
        
        ILogger SetLevel(LogLevel level);
        ILogger Disable();
    }
}