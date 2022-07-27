using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LogVoid : LogBase
    {
        public LogVoid(string prefix, LogLevel level) : base(prefix, level, false, false)
        {
        }

        /*
         * Debug.
         */

        public override void Debug(string message)                                                                         { }
        public override void Debug(Exception exception)                                                                    { }
        public override void Debug(Func<string> callback)                                                                  { }
        public override void Debug<T1>(Func<T1, string> callback, T1 param01)                                              { }
        public override void Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                          { }
        public override void Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)      { }
        public override void Debug(Action<ILogDebug> callback)                                                             { }
        public override void Debug<T1>(Action<ILogDebug, T1> callback, T1 param01)                                         { }
        public override void Debug<T1, T2>(Action<ILogDebug, T1, T2> callback, T1 param01, T2 param02)                     { }
        public override void Debug<T1, T2, T3>(Action<ILogDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }

        /*
         * Warn.
         */

        public override void Warn(string message)                                                                        { }
        public override void Warn(Exception exception)                                                                   { }
        public override void Warn(Func<string> callback)                                                                 { }
        public override void Warn<T1>(Func<T1, string> callback, T1 param01)                                             { }
        public override void Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                         { }
        public override void Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)     { }
        public override void Warn(Action<ILogWarn> callback)                                                             { }
        public override void Warn<T1>(Action<ILogWarn, T1> callback, T1 param01)                                         { }
        public override void Warn<T1, T2>(Action<ILogWarn, T1, T2> callback, T1 param01, T2 param02)                     { }
        public override void Warn<T1, T2, T3>(Action<ILogWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }

        /*
         * Error.
         */

        public override void Error(string message)                                                                         { }
        public override void Error(Exception exception)                                                                    { }
        public override void Error(Func<string> callback)                                                                  { }
        public override void Error<T1>(Func<T1, string> callback, T1 param01)                                              { }
        public override void Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)                          { }
        public override void Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)      { }
        public override void Error(Action<ILogError> callback)                                                             { }
        public override void Error<T1>(Action<ILogError, T1> callback, T1 param01)                                         { }
        public override void Error<T1, T2>(Action<ILogError, T1, T2> callback, T1 param01, T2 param02)                     { }
        public override void Error<T1, T2, T3>(Action<ILogError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03) { }
    }
}