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
        [Log(LogLevel.Warning)] public ILog             Log        { get; set; }
        [Inject]                public IEventDispatcher Dispatcher { get; set; }

        public Screen CurrentScreen   { get; private set; }
        public bool   HasShownScreens => _openScreens.Count > 0;

        private readonly List<Screen> _openScreens;

        public ScreensController()
        {
            _openScreens = new List<Screen>(4);
        }

        /*
         * Show.
         */

        public void Show(Screen screen)
        {
            Show(screen, ScreenBehavior.Replace);
        }

        public void Show(Screen screen, ScreenBehavior behavior)
        {
            Log.Debug((s, b) => $"Show. {s} {b}", screen, behavior);

            if (screen == CurrentScreen)
            {
                Log.Warn(s => $"Screen already shown: {s}", screen);
                return;
            }

            var previousScreen = CurrentScreen;
            if (behavior == ScreenBehavior.Replace && CurrentScreen != null)
            {
                HideScreenImpl(CurrentScreen);
                CurrentScreen = null;
            }

            var instance = GetInstance(screen, UIControlOptions.Instantiate | UIControlOptions.Activate, out var isNewInstance);
            if (instance == null)
            {
                Log.Error(s => $"Failed to instantiate screen: {s}", screen);
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
                CurrentScreen = screen;
                Dispatcher.Dispatch(ScreenEvent.Shown, screen);
            }

            if (behavior == ScreenBehavior.OpenOnTop && previousScreen != null)
                Dispatcher.Dispatch(ScreenEvent.Hidden, previousScreen);
        }

        /*
         * Hide.
         */

        public void Hide(Screen screen)
        {
            Log.Debug(s => $"Hide. {s}", screen);

            HideScreenImpl(screen);

            if (CurrentScreen == screen)
                CurrentScreen = null;

            if (_openScreens.Count > 0)
                Show(_openScreens[0]);
        }
        
        /*
         * Check.
         */

        public bool CheckScreenIsActive(Screen screen)
        {
            return _openScreens.Contains(screen);
        }

        /*
         * Private.
         */

        private void HideScreenImpl(Screen screen)
        {
            Log.Debug(s => $"HideScreenImpl. {s}", screen);

            if (!_openScreens.Contains(screen))
            {
                Log.Error(s => $"Specified screen is not shown: {s}", screen);
                return;
            }

            if (!Deactivate(screen, out var destroyed) || !_openScreens.Remove(screen))
                return;

            Log.Debug((s, d) => $"Hidden: {s} destroyed={d}", screen, destroyed);

            Dispatcher.Dispatch(ScreenEvent.Hidden, screen);

            if (destroyed)
                Dispatcher.Dispatch(ScreenEvent.Destroyed, screen);
        }
    }
}