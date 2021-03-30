using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal abstract class LoggerBase : ILogger, ILoggerError, ILoggerWarn, ILoggerDebug
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

        public ILoggerDebug Debug(string message)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatMessage(message));
            return this;
        }

        public ILoggerDebug Debug(Exception exception)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatException(exception));
            return this;
        }

        public ILoggerDebug Debug(Func<string> callback)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatMessage(callback.Invoke()));
            return this;
        }

        public ILoggerDebug Debug<T1>(Func<T1, string> callback, T1 param01)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatMessage(callback.Invoke(param01)));
            return this;
        }

        public ILoggerDebug Debug<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatMessage(callback.Invoke(param01, param02)));
            return this;
        }

        public ILoggerDebug Debug<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                DebugImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
            return this;
        }

        public ILoggerDebug Debug(Action<ILoggerDebug> callback)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                callback.Invoke(this);
            return this;
        }

        public ILoggerDebug Debug<T1>(Action<ILoggerDebug, T1> callback, T1 param01)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                callback.Invoke(this, param01);
            return this;
        }

        public ILoggerDebug Debug<T1, T2>(Action<ILoggerDebug, T1, T2> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                callback.Invoke(this, param01, param02);
            return this;
        }

        public ILoggerDebug Debug<T1, T2, T3>(Action<ILoggerDebug, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Debug) == LogLevel.Debug)
                callback.Invoke(this, param01, param02, param03);
            return this;
        }

        /*
         * Warn.
         */

        public ILoggerWarn Warn(string message)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatMessage(message));
            return this;
        }

        public ILoggerWarn Warn(Exception exception)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatException(exception));
            return this;
        }

        public ILoggerWarn Warn(Func<string> callback)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatMessage(callback.Invoke()));
            return this;
        }

        public ILoggerWarn Warn<T1>(Func<T1, string> callback, T1 param01)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatMessage(callback.Invoke(param01)));
            return this;
        }

        public ILoggerWarn Warn<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatMessage(callback.Invoke(param01, param02)));
            return this;
        }

        public ILoggerWarn Warn<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                WarningImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
            return this;
        }

        public ILoggerWarn Warn(Action<ILoggerWarn> callback)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                callback.Invoke(this);
            return this;
        }

        public ILoggerWarn Warn<T1>(Action<ILoggerWarn, T1> callback, T1 param01)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                callback.Invoke(this, param01);
            return this;
        }

        public ILoggerWarn Warn<T1, T2>(Action<ILoggerWarn, T1, T2> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                callback.Invoke(this, param01, param02);
            return this;
        }

        public ILoggerWarn Warn<T1, T2, T3>(Action<ILoggerWarn, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Warning) == LogLevel.Warning)
                callback.Invoke(this, param01, param02, param03);
            return this;
        }

        /*
         * Error.
         */

        public ILoggerError Error(string message)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatMessage(message));
            return this;
        }

        public ILoggerError Error(Exception exception)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatException(exception));
            return this;
        }

        public ILoggerError Error(Func<string> callback)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatMessage(callback.Invoke()));
            return this;
        }

        public ILoggerError Error<T1>(Func<T1, string> callback, T1 param01)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatMessage(callback.Invoke(param01)));
            return this;
        }

        public ILoggerError Error<T1, T2>(Func<T1, T2, string> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatMessage(callback.Invoke(param01, param02)));
            return this;
        }

        public ILoggerError Error<T1, T2, T3>(Func<T1, T2, T3, string> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                ErrorImpl(FormatMessage(callback.Invoke(param01, param02, param03)));
            return this;
        }

        public ILoggerError Error(Action<ILoggerError> callback)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                callback.Invoke(this);
            return this;
        }

        public ILoggerError Error<T1>(Action<ILoggerError, T1> callback, T1 param01)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                callback.Invoke(this, param01);
            return this;
        }

        public ILoggerError Error<T1, T2>(Action<ILoggerError, T1, T2> callback, T1 param01, T2 param02)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                callback.Invoke(this, param01, param02);
            return this;
        }

        public ILoggerError Error<T1, T2, T3>(Action<ILoggerError, T1, T2, T3> callback, T1 param01, T2 param02, T3 param03)
        {
            if ((_level & LogLevel.Error) == LogLevel.Error)
                callback.Invoke(this, param01, param02, param03);
            return this;
        }

        /*
         * Public.
         */

        public ILogger SetLevel(LogLevel level)
        {
            _level = level;
            return this;
        }

        public ILogger Disable()
        {
            return SetLevel(LogLevel.None);
        }

        /*
         * Protected.
         */

        protected abstract void DebugImpl(string message);
        protected abstract void WarningImpl(string message);
        protected abstract void ErrorImpl(string message);

        /*
         * Private.
         */

        private string FormatMessage(object message)
        {
            return $"{_prefix}: {message}";
        }

        private string FormatException(Exception exception)
        {
            return $"{_prefix}: {exception.GetType().Name}: {exception.Message}";
        }
    }
}