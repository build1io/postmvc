#if UNITY_WEBGL
    using System.Runtime.InteropServices;
#endif

namespace Build1.PostMVC.Extensions.Unity.Modules.Logging.Impl
{
    internal sealed class LoggerWebGL : LoggerBase
    {
        #if UNITY_WEBGL
        
            [DllImport("__Internal")]
            private static extern void LogDebug(string message);

            [DllImport("__Internal")]
            private static extern void LogWarning(string message);
            
            [DllImport("__Internal")]
            private static extern void LogError(string message);
        
        #endif

        public LoggerWebGL(string prefix, LogLevel level) : base(prefix, level)
        {
        }
        
        protected override void DebugImpl(string message)
        {
            #if UNITY_WEBGL
                LogDebug(message);
            #endif
        }

        protected override void WarningImpl(string message)
        {
            #if UNITY_WEBGL
                LogWarning(message);
            #endif
        }

        protected override void ErrorImpl(string message)
        {
            #if UNITY_WEBGL
                LogError(message);
            #endif
        }
    }
}