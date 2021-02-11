namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LoggerDebug : LoggerBase
    {
        public LoggerDebug(string prefix, LogLevel level) : base(prefix, level)
        {
        }
        
        protected override void DebugImpl(string message)
        {
            UnityEngine.Debug.Log(message);
        }

        protected override void WarningImpl(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }

        protected override void ErrorImpl(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
        
    }
}