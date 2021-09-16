using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.FullScreen.Impl
{
    internal sealed class FullScreenController : IFullScreenController
    {
        [Log(LogLevel.All)] public ILog             Log        { get; set; }
        [Inject]            public IEventDispatcher Dispatcher { get; set; }

        public bool IsInFullScreen { get; private set; }

        public void ToggleFullScreen()
        {
            if (Application.isEditor)
            {
                Log.Warn("Full screen is not supported in Editor.");
                return;
            }

            // Fullscreen will be out of sync if a player exits fullscreen by himself.

            Screen.fullScreen = !Screen.fullScreen;
            IsInFullScreen = Screen.fullScreen;

            Dispatcher.Dispatch(FullScreenEvent.Changed, IsInFullScreen);
        }
    }
}