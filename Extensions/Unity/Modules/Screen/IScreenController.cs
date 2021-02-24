namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public interface IScreenController
    {
        void Show(Screen screen);
        void Show(Screen screen, ScreenBehavior behavior);
        void Hide(Screen screen);

        bool ScreenIsActive(Screen screen);
    }
}