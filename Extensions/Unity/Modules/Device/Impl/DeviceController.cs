using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using Build1.PostMVC.Extensions.Unity.Utils;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device.Impl
{
    public sealed class DeviceController : IDeviceController
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }

        public DeviceType CurrentDeviceType
        {
            get
            {
                #if UNITY_EDITOR
                    // This will make controller to recalculate this on every call.
                    // It helps to work in editor while developer switches device in Simulator.
                    _currentDeviceType = DeviceUtil.GetDeviceType(CurrentDevicePlatform);
                #endif
                
                return _currentDeviceType;
            }
            private set
            {
                _currentDeviceType = value;
            }
        }

        public DevicePlatform CurrentDevicePlatform { get; private set; }

        public bool IsMobile => CurrentDevicePlatform == DevicePlatform.iOS ||
                                CurrentDevicePlatform == DevicePlatform.Android;

        public bool IsDesktop => CurrentDevicePlatform == DevicePlatform.OSX ||
                                 CurrentDevicePlatform == DevicePlatform.Windows ||
                                 CurrentDevicePlatform == DevicePlatform.WebGL;

        public bool IsPhone  => CurrentDeviceType == DeviceType.Phone;
        public bool IsTablet => CurrentDeviceType == DeviceType.Tablet;

        private RuntimePlatform _platform;
        private bool            _platformSet;

        private DeviceType _currentDeviceType;

        [PostConstruct]
        public void PostConstruct()
        {
            UpdateDeviceInfo();
        }

        /*
         * Public.
         */

        public void SetPlatform(RuntimePlatform platform)
        {
            _platform = platform;
            _platformSet = true;

            UpdateDeviceInfo();
        }

        public T GetConfiguration<T>(IEnumerable<T> configurations) where T : IDeviceDependentConfiguration
        {
            foreach (var configuration in configurations)
            {
                if (configuration.DevicePlatform != DevicePlatform.Any && configuration.DevicePlatform != CurrentDevicePlatform)
                    continue;
                if (configuration.DeviceType != DeviceType.Any && configuration.DeviceType != CurrentDeviceType)
                    continue;
                return configuration;
            }

            throw new Exception($"No suitable configuration found: platform: {CurrentDevicePlatform} device: {CurrentDeviceType}");
        }

        /*
         * Private.
         */

        private void UpdateDeviceInfo()
        {
            CurrentDevicePlatform = GetCurrentDevicePlatform();
            CurrentDeviceType = DeviceUtil.GetDeviceType(CurrentDevicePlatform);

            Log.Debug((p, t) => $"Platform: {p} DeviceType: {t}", CurrentDevicePlatform, CurrentDeviceType);
        }

        private DevicePlatform GetCurrentDevicePlatform()
        {
            var isEditor = Application.platform == RuntimePlatform.OSXEditor ||
                           Application.platform == RuntimePlatform.WindowsEditor;

            if (_platformSet || !isEditor)
            {
                var platform = _platformSet ? _platform : Application.platform;
                switch (platform)
                {
                    case RuntimePlatform.IPhonePlayer: return DevicePlatform.iOS;
                    case RuntimePlatform.Android:      return DevicePlatform.Android;

                    case RuntimePlatform.WebGLPlayer:   return DevicePlatform.WebGL;
                    case RuntimePlatform.OSXPlayer:     return DevicePlatform.OSX;
                    case RuntimePlatform.WindowsPlayer: return DevicePlatform.Windows;

                    case RuntimePlatform.OSXEditor:
                    case RuntimePlatform.WindowsEditor:
                        // If platform wasn't processed correctly and we're currently in Unity Editor, break.
                        // It'll try processing by the build target from editor settings.
                        break;

                    default:
                        throw new Exception($"Unsupported platform: {platform}");
                }
            }

            #if UNITY_EDITOR
            {
                var target = UnityEditor.EditorUserBuildSettings.activeBuildTarget;
                switch (target)
                {
                    case UnityEditor.BuildTarget.iOS:     return DevicePlatform.iOS;
                    case UnityEditor.BuildTarget.Android: return DevicePlatform.Android;

                    case UnityEditor.BuildTarget.WebGL:         return DevicePlatform.WebGL;
                    case UnityEditor.BuildTarget.StandaloneOSX: return DevicePlatform.OSX;
                    
                    case UnityEditor.BuildTarget.StandaloneWindows:
                    case UnityEditor.BuildTarget.StandaloneWindows64: return DevicePlatform.Windows;

                    default:
                        throw new Exception($"Unsupported build target: {target}");
                }
            }
            #endif

            throw new Exception($"Unable to process getting current device platform.");
        }
    }
}