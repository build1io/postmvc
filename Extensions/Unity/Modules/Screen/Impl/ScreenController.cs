using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen.Impl
{
    public sealed class ScreenController : UIControlsController<Screen, ScreenConfig>, IScreenController
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        private readonly List<Screen> _currentScreens;

        public ScreenController()
        {
            _currentScreens = new List<Screen>(4);
        }

        public void Show(Screen screen)
        {
            Show(screen, ScreenBehavior.Default);
        }

        public void Show(Screen screen, ScreenBehavior behavior)
        {
            if (_currentScreens.Count > 0 && behavior == ScreenBehavior.Default)
            {
                // Hides all active screens if behavior is ScreenBehavior.Default.
                for (var i = _currentScreens.Count - 1; i >= 0; i--)
                    Hide(_currentScreens[i]);
            }

            var instance = GetInstance(screen, UIControlOptions.Instantiate | UIControlOptions.Activate);
            if (instance == null)
                return;

            if (behavior == ScreenBehavior.OpenOnTop)
            {
                _currentScreens.Insert(0, screen);
                
                // We don't need to move object in hierarchy as it'll be added on top of everything on the layer.
            }
            else
            {
                _currentScreens.Add(screen);    
            }
            
            Dispatcher.Dispatch(ScreenEvent.Open, screen);
        }

        public void Hide(Screen screen)
        {
            if (Deactivate(screen) && _currentScreens.Remove(screen))
                Dispatcher.Dispatch(ScreenEvent.Closed, screen);
        }

        public bool ScreenIsActive(Screen screen)
        {
            return _currentScreens.Contains(screen) && CheckControlIsActive(screen);
        }
    }
}