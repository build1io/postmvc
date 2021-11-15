using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.DeviceOrientation
{
    public static class DeviceOrientationEvent
    {
        public static readonly Event<DeviceOrientation> Changed = new Event<DeviceOrientation>();
    }
}