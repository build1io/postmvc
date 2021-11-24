namespace Build1.PostMVC.Extensions.Unity.Modules.Orientation
{
    public interface IOrientationController
    {
        DeviceOrientation DeviceOrientation { get; }
        ScreenOrientation ScreenOrientation { get; }
        
        void SetAvailableOrientations(ScreenOrientation orientation);
    }
}