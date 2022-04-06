using System;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device.Impl
{
    public sealed class DeviceControllerRuntime : DeviceControllerBase
    {
        protected override DevicePlatform GetCurrentPlatform()
        {
            var platform = Application.platform;
            return platform switch
            {
                RuntimePlatform.IPhonePlayer  => DevicePlatform.iOS,
                RuntimePlatform.Android       => DevicePlatform.Android,
                RuntimePlatform.WebGLPlayer   => DevicePlatform.WebGL,
                RuntimePlatform.OSXPlayer     => DevicePlatform.OSX,
                RuntimePlatform.OSXEditor     => DevicePlatform.OSX,
                RuntimePlatform.WindowsPlayer => DevicePlatform.Windows,
                RuntimePlatform.WindowsEditor => DevicePlatform.Windows,
                RuntimePlatform.LinuxPlayer   => DevicePlatform.Linux,
                RuntimePlatform.LinuxEditor   => DevicePlatform.Linux,
                RuntimePlatform.XboxOne       => DevicePlatform.XboxOne,
                RuntimePlatform.PS4           => DevicePlatform.PS4,
                RuntimePlatform.PS5           => DevicePlatform.PS5,
                RuntimePlatform.Switch        => DevicePlatform.Switch,
                RuntimePlatform.tvOS          => DevicePlatform.tvOS,
                _                             => throw new Exception($"Unsupported platform: {platform}")
            };
        }
    }
}