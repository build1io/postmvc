using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LogVoid : ILog
    {
        /*
         * Debug.
         */

        public void Debug(string message)                                                                         { }
        public void Debug(Exception exception)                                                                    { }
        public void Debug(Func<string> callback)                                                                  { }
        public void Debug<T1>(Func<T1, string> callback, T1 param01)                                              { }
        public void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                          { }
        public void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)      { }
        public void Debug(Action<ILogDebug> callback)                                                             { }
        public void Debug<T1>(Action<ILogDebug, T1> callback, T1 param01)                                         { }
        public void Debug<T1, T2>(Action<ILogDebug, T1, T2> callback, T1 param01, T2 param02)                     { }
        public void Debug<T1, T2, T3>(Action<ILogDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }

        /*
         * Warn.
         */

        public void Warn(string message)                                                                        { }
        public void Warn(Exception exception)                                                                   { }
        public void Warn(Func<string> callback)                                                                 { }
        public void Warn<T1>(Func<T1, string> callback, T1 param01)                                             { }
        public void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                         { }
        public void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)     { }
        public void Warn(Action<ILogWarn> callback)                                                             { }
        public void Warn<T1>(Action<ILogWarn, T1> callback, T1 param01)                                         { }
        public void Warn<T1, T2>(Action<ILogWarn, T1, T2> callback, T1 param01, T2 param02)                     { }
        public void Warn<T1, T2, T3>(Action<ILogWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }

        /*
         * Error.
         */

        public void Error(string message)                                                                         { }
        public void Error(Exception exception)                                                                    { }
        public void Error(Func<string> callback)                                                                  { }
        public void Error<T1>(Func<T1, string> callback, T1 param01)                                              { }
        public void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                          { }
        public void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)      { }
        public void Error(Action<ILogError> callback)                                                             { }
        public void Error<T1>(Action<ILogError, T1> callback, T1 param01)                                         { }
        public void Error<T1, T2>(Action<ILogError, T1, T2> callback, T1 param01, T2 param02)                     { }
        public void Error<T1, T2, T3>(Action<ILogError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }
        
        /*
         * Other.
         */

        public void SetPrefix(string prefix) { }
        public void SetLevel(LogLevel level) { }
        public void Disable()                { }
    }
}