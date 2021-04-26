using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens.Impl
{
    internal sealed class ScreensController : UIControlsController<Screen, ScreenConfig>, IScreensController
    {
        [Logger(LogLevel.Warning)] public ILogger          Logger     { get; set; }
        [Inject]                   public IEventDispatcher Dispatcher { get; set; }

        public bool HasShownScreens => _openScreens.Count > 0;

        private readonly List<Screen> _openScreens;

        public ScreensController()
        {
            _openScreens = new List<Screen>(4);
        }

        /*
         * Showing.
         */

        public void Show(Screen screen)
        {
            Show(screen, ScreenBehavior.Default);
        }

        public void Show(Screen screen, ScreenBehavior behavior)
        {
            if (behavior == ScreenBehavior.Default && HasShownScreens)
                HideAll();

            var instance = GetInstance(screen, UIControlOptions.Instantiate | UIControlOptions.Activate);
            if (instance == null)
                return;

            if (behavior == ScreenBehavior.OpenOnTop)
            {
                _openScreens.Insert(0, screen);

                // No need to move object in hierarchy as it'll be added on top of everything on the layer.
            }
            else
            {
                _openScreens.Add(screen);
            }

            Dispatcher.Dispatch(ScreenEvent.Open, screen);
        }

        /*
         * Hiding.
         */

        public void Hide(Screen screen)
        {
            if (!_openScreens.Contains(screen))
            {
                Logger.Error(s => $"Specified screen is not shown: {s}", screen);
                return;
            }

            if (Deactivate(screen) && _openScreens.Remove(screen))
            {
                Dispatcher.Dispatch(ScreenEvent.Closed, screen);
                return;
            }

            Logger.Error(s => $"Failed to deactivate screen: {s}", screen);
        }

        public void HideAll()
        {
            for (var i = _openScreens.Count - 1; i >= 0; i--)
                Hide(_openScreens[i]);
        }

        /*
         * Other.
         */

        public bool ScreenIsActive(Screen screen)
        {
            return _openScreens.Contains(screen);
        }
    }
}