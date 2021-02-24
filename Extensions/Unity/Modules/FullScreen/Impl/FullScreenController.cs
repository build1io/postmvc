using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.FullScreen.Impl
{
    internal sealed class FullScreenController : IFullScreenController
    {
        [Logger(LogLevel.All)] public ILogger          Logger     { get; set; }
        [Inject]               public IEventDispatcher Dispatcher { get; set; }
        
        public bool IsInFullScreen { get; private set; }
        
        public void ToggleFullScreen()
        {
            if (Application.isEditor)
            {
                Logger.Warn(() => "Full screen is not supported in Editor.");
                return;
            }
            
            // Fullscreen will be out of sync if a player exits fullscreen by himself.

            UnityEngine.Screen.fullScreen = !UnityEngine.Screen.fullScreen;
            IsInFullScreen = UnityEngine.Screen.fullScreen;

            Dispatcher.Dispatch(FullScreenEvent.Changed, IsInFullScreen);
        }
    }
}