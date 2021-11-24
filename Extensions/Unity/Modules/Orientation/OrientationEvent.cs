using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Orientation
{
    public static class OrientationEvent
    {
        public static readonly Event<DeviceOrientation> DeviceOrientationChanged = new Event<DeviceOrientation>();
        public static readonly Event<ScreenOrientation> ScreenOrientationChanged = new Event<ScreenOrientation>();
    }
}