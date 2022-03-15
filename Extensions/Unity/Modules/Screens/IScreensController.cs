namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public interface IScreensController
    {
        Screen CurrentScreen   { get; }
        bool   HasShownScreens { get; }

        void Show(Screen screen);
        void Show(Screen screen, ScreenBehavior behavior);
        
        void Hide(Screen screen);
        
        bool CheckScreenIsActive(Screen screen);
        bool CheckScreenIsCurrent(Screen screen);
    }
}