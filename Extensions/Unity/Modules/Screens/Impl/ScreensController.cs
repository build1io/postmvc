using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens.Impl
{
    public sealed class ScreensController : UIControlsController<Screen, ScreenConfig>, IScreensController
    {
        [Logger(LogLevel.Warning)] public ILogger          Logger     { get; set; }
        [Inject]                   public IEventDispatcher Dispatcher { get; set; }

        public bool HasShownScreens => _openScreens.Count > 0;

        private          Screen       _currentScreen;
        private readonly List<Screen> _openScreens;

        public ScreensController()
        {
            _openScreens = new List<Screen>(4);
        }
        
        /*
         * Check.
         */

        public bool ScreenIsActive(Screen screen)
        {
            return _openScreens.Contains(screen);
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
            Logger.Debug((s, b) => $"Show. {s} {b}", screen, behavior);
            
            if (screen == _currentScreen)
            {
                Logger.Warn(s => $"Screen already shown: {s}", screen);
                return;
            }

            var previousScreen = _currentScreen;
            if (behavior == ScreenBehavior.Default && _currentScreen != null)
            {
                HideScreenImpl(_currentScreen);
                _currentScreen = null;
            }
            
            var instance = GetInstance(screen, UIControlOptions.Instantiate | UIControlOptions.Activate, out var isNewInstance);
            if (instance == null)
            {
                Logger.Error(s => $"Failed to instantiate screen: {s}", screen);
                return;
            }

            if (!_openScreens.Contains(screen))
            {
                if (behavior == ScreenBehavior.OpenOnTop)
                {
                    _openScreens.Insert(0, screen);

                    // No need to move object in hierarchy as it'll be added on top of everything on the layer.
                }
                else
                {
                    _openScreens.Add(screen);
                }
            }

            if (isNewInstance)
                Dispatcher.Dispatch(ScreenEvent.Created, screen);

            if (behavior != ScreenBehavior.OpenInBackground)
            {
                _currentScreen = screen;
                Dispatcher.Dispatch(ScreenEvent.Shown, screen);
            }

            if (behavior == ScreenBehavior.OpenOnTop && previousScreen != null)
                Dispatcher.Dispatch(ScreenEvent.Hidden, previousScreen);
        }

        /*
         * Hiding.
         */

        public void Hide(Screen screen)
        {
            Logger.Debug(s => $"Hide. {s}", screen);
            
            HideScreenImpl(screen);

            if (_currentScreen == screen)
                _currentScreen = null;

            if (_openScreens.Count > 0)
                Show(_openScreens[0]);
        }

        /*
         * Private.
         */

        private void HideScreenImpl(Screen screen)
        {
            Logger.Debug(s => $"HideScreenImpl. {s}", screen);
            
            if (!_openScreens.Contains(screen))
            {
                Logger.Error(s => $"Specified screen is not shown: {s}", screen);
                return;
            }

            if (!Deactivate(screen, out var destroyed) || !_openScreens.Remove(screen))
                return;
            
            Logger.Debug((s, d) => $"Hidden: {s} destroyed={d}", screen, destroyed);
            
            Dispatcher.Dispatch(ScreenEvent.Hidden, screen);
            
            if (destroyed)
                Dispatcher.Dispatch(ScreenEvent.Destroyed, screen);
        }
    }
}