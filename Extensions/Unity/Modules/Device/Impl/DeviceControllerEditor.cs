#if UNITY_EDITOR

using System;
using UnityEditor;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device.Impl
{
    public sealed class DeviceControllerEditor : DeviceControllerBase
    {
        protected override DevicePlatform GetCurrentPlatform()
        {
            var target = EditorUserBuildSettings.activeBuildTarget;
            return target switch
            {
                BuildTarget.iOS                 => DevicePlatform.iOS,
                BuildTarget.Android             => DevicePlatform.Android,
                BuildTarget.WebGL               => DevicePlatform.WebGL,
                BuildTarget.StandaloneOSX       => DevicePlatform.OSX,
                BuildTarget.StandaloneWindows   => DevicePlatform.Windows,
                BuildTarget.StandaloneWindows64 => DevicePlatform.Windows,
                BuildTarget.StandaloneLinux64   => DevicePlatform.Linux,
                BuildTarget.PS4                 => DevicePlatform.PS4,
                BuildTarget.PS5                 => DevicePlatform.PS5,
                BuildTarget.XboxOne             => DevicePlatform.XboxOne,
                BuildTarget.tvOS                => DevicePlatform.tvOS,
                BuildTarget.Switch              => DevicePlatform.Switch,
                _                               => throw new Exception($"Unsupported build target: {target}")
            };
        }
    }
}

#endif