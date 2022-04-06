using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    public interface IDeviceController
    {
        DevicePlatform          DevicePlatform          { get; }
        DeviceType              DeviceType              { get; }
        DeviceOrientation       DeviceOrientation       { get; }
        DeviceScreenOrientation DeviceScreenOrientation { get; }

        bool IsMobile  { get; }
        bool IsDesktop { get; }
        bool IsConsole { get; }
        bool IsTV      { get; }

        bool IsPhone  { get; }
        bool IsTablet { get; }
        bool IsWeb    { get; }

        T GetConfiguration<T>(IEnumerable<T> configurations) where T : IDeviceDependentConfiguration;

        void SetAvailableOrientations(DeviceScreenOrientation orientation);
    }
}