using System.Collections.Generic;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    public interface IDeviceController
    {
        DevicePlatform CurrentDevicePlatform { get; }
        DeviceType     CurrentDeviceType     { get; }

        bool IsMobile  { get; }
        bool IsDesktop { get; }

        bool IsPhone  { get; }
        bool IsTablet { get; }

        void SetPlatform(RuntimePlatform platform);
        T    GetConfiguration<T>(IEnumerable<T> configurations) where T : IDeviceDependentConfiguration;
    }
}