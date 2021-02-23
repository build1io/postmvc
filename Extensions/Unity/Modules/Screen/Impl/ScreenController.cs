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

        public void Show(Screen screen, ScreenBehavior behavior)
        {
            if (behavior == ScreenBehavior.Default && _currentScreens.Count > 0)
                Hide(_currentScreens[0]);

            var instance = GetInstance(screen, UIControlOptions.Instantiate | UIControlOptions.Activate);
            if (instance == null)
                return;
            
            _currentScreens.Add(screen);
            Dispatcher.Dispatch(ScreenEvent.Open, screen);
        }

        public void Hide(Screen screen)
        {
            if (_currentScreens.Remove(screen) && Deactivate(screen))
                Dispatcher.Dispatch(ScreenEvent.Closed, screen);
        }

        public bool ScreenIsActive(Screen screen)
        {
            return _currentScreens.Contains(screen);
        }
    }
}