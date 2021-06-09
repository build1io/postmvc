namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public interface IScreensController
    {
        bool HasShownScreens { get; }

        bool ScreenIsActive(Screen screen);
        
        void Show(Screen screen);
        void Show(Screen screen, ScreenBehavior behavior);
        
        void Hide(Screen screen);
    }
}