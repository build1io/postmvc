namespace Build1.PostMVC.Extensions.Unity.Modules.DeviceOrientation
{
    public interface IDeviceOrientationController
    {
        DeviceOrientation Orientation { get; }
        
        void SetAvailableOrientations(DeviceOrientation orientation);
    }
}