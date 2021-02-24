namespace Build1.PostMVC.Extensions.Unity.Modules.FullScreen
{
    public interface IFullScreenController
    {
        bool IsInFullScreen { get; }
        
        void ToggleFullScreen();
    }
}