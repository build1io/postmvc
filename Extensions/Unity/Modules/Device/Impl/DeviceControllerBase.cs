using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.App;
using Build1.PostMVC.Extensions.Unity.Modules.Async;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device.Impl
{
    public abstract class DeviceControllerBase : IDeviceController
    {
        private const float OrientationCheckTimeout = 0.2F;

        [Log(LogLevel.Warning)] public ILog           Log           { get; set; }
        [Inject]                public IEventMap      EventMap      { get; set; }
        [Inject]                public IAsyncResolver AsyncResolver { get; set; }

        public DevicePlatform          DevicePlatform          { get; private set; }
        public DeviceType              DeviceType              { get; private set; }
        public DeviceOrientation       DeviceOrientation       { get; private set; }
        public DeviceScreenOrientation DeviceScreenOrientation { get; private set; }

        public bool IsMobile => DevicePlatform == DevicePlatform.iOS ||
                                DevicePlatform == DevicePlatform.Android;

        public bool IsDesktop => DevicePlatform == DevicePlatform.OSX ||
                                 DevicePlatform == DevicePlatform.Windows ||
                                 DevicePlatform == DevicePlatform.Linux ||
                                 DevicePlatform == DevicePlatform.WebGL;

        public bool IsConsole => DevicePlatform == DevicePlatform.PS4 ||
                                 DevicePlatform == DevicePlatform.PS5 ||
                                 DevicePlatform == DevicePlatform.XboxOne ||
                                 DevicePlatform == DevicePlatform.Switch;

        public bool IsTV => DevicePlatform == DevicePlatform.tvOS;

        public bool IsPhone  => DeviceType == DeviceType.Phone;
        public bool IsTablet => DeviceType == DeviceType.Tablet;
        public bool IsWeb    => DevicePlatform == DevicePlatform.WebGL;

        private int                           _intervalId;
        private UnityEngine.DeviceOrientation _deviceOrientation;
        private ScreenOrientation             _screenOrientation;

        [PostConstruct]
        public void PostConstruct()
        {
            DevicePlatform = GetCurrentPlatform();
            DeviceType = GetDeviceType(DevicePlatform);
            DeviceOrientation = ToDeviceOrientation(Input.deviceOrientation);
            DeviceScreenOrientation = ToScreenOrientation(Screen.orientation);

            Log.Debug((p, t) => $"Platform: {p} DeviceType: {t}", DevicePlatform, DeviceType);
            Log.Debug(() => "Diagonal: " + GetDiagonalInches() + " AspectRatio: " + GetAspectRatio());

            EventMap.Map(AppEvent.Pause, OnAppPause);

            StartCheck();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            EventMap.UnmapAll();

            StopCheck();
        }

        /*
         * Public.
         */

        public T GetConfiguration<T>(IEnumerable<T> configurations) where T : IDeviceDependentConfiguration
        {
            foreach (var configuration in configurations)
            {
                if (configuration.DevicePlatform != DevicePlatform.Any && configuration.DevicePlatform != DevicePlatform)
                    continue;
                if (configuration.DeviceType != DeviceType.Any && configuration.DeviceType != DeviceType)
                    continue;
                return configuration;
            }

            throw new Exception($"No suitable configuration found: platform: {DevicePlatform} device: {DeviceType}");
        }

        public void SetAvailableOrientations(DeviceScreenOrientation orientations)
        {
            Log.Debug(o => $"SetAvailableOrientations: {o}", orientations);

            switch (orientations)
            {
                case DeviceScreenOrientation.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    return;
                case DeviceScreenOrientation.PortraitUpsideDown:
                    Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                    return;
                case DeviceScreenOrientation.LandscapeLeft:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    return;
                case DeviceScreenOrientation.LandscapeRight:
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    return;
            }

            var autorotateToPortrait = (orientations & DeviceScreenOrientation.Portrait) == DeviceScreenOrientation.Portrait;
            var autorotateToPortraitUpsideDown = (orientations & DeviceScreenOrientation.PortraitUpsideDown) == DeviceScreenOrientation.PortraitUpsideDown;
            var autorotateToLandscapeLeft = (orientations & DeviceScreenOrientation.LandscapeLeft) == DeviceScreenOrientation.LandscapeLeft;
            var autorotateToLandscapeRight = (orientations & DeviceScreenOrientation.LandscapeRight) == DeviceScreenOrientation.LandscapeRight;

            Screen.autorotateToPortrait = autorotateToPortrait;
            Screen.autorotateToPortraitUpsideDown = autorotateToPortraitUpsideDown;
            Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
            Screen.autorotateToLandscapeRight = autorotateToLandscapeRight;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        /*
         * Protected.
         */

        protected abstract DevicePlatform GetCurrentPlatform();

        /*
         * Private.
         */

        private void StartCheck()
        {
            Log.Debug("StartCheck");

            if (_intervalId == AsyncResolver.DefaultCallId)
                _intervalId = AsyncResolver.IntervalCall(Check, OrientationCheckTimeout);
        }

        private void StopCheck()
        {
            Log.Debug("StopCheck");

            AsyncResolver.CancelCall(ref _intervalId);
        }

        private void Check()
        {
            var deviceType = GetDeviceType(DevicePlatform);
            if (DeviceType != deviceType)
            {
                Log.Debug(o => $"Device Type Changed: {o}", deviceType);

                DeviceType = deviceType;

                EventMap.Dispatch(DeviceEvent.DeviceTypeChanged, DeviceType);
            }

            if (_deviceOrientation != Input.deviceOrientation)
            {
                Log.Debug(o => $"Device orientation chanced: {o}", Input.deviceOrientation);

                _deviceOrientation = Input.deviceOrientation;
                DeviceOrientation = ToDeviceOrientation(Input.deviceOrientation);

                EventMap.Dispatch(DeviceEvent.DeviceOrientationChanged, DeviceOrientation);
            }

            if (_screenOrientation != Screen.orientation)
            {
                Log.Debug(o => $"Screen orientation changed: {o}", Screen.orientation);

                _screenOrientation = Screen.orientation;
                DeviceScreenOrientation = ToScreenOrientation(_screenOrientation);

                EventMap.Dispatch(DeviceEvent.ScreenOrientationChanged, DeviceScreenOrientation);
            }
        }

        private DeviceType GetDeviceType(DevicePlatform platform)
        {
            return platform switch
            {
                DevicePlatform.iOS     => GetMobileDeviceType(),
                DevicePlatform.Android => GetMobileDeviceType(),
                DevicePlatform.OSX     => DeviceType.Desktop,
                DevicePlatform.Windows => DeviceType.Desktop,
                DevicePlatform.Linux   => DeviceType.Desktop,
                DevicePlatform.WebGL   => DeviceType.Desktop,
                DevicePlatform.XboxOne => DeviceType.Console,
                DevicePlatform.PS4     => DeviceType.Console,
                DevicePlatform.PS5     => DeviceType.Console,
                DevicePlatform.Switch  => DeviceType.Console,
                DevicePlatform.tvOS    => DeviceType.TV,
                _                      => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
            };
        }

        private DeviceType GetMobileDeviceType()
        {
            var diagonal = GetDiagonalInches();
            var aspect = GetAspectRatio();
            if (aspect >= 1.77)
                return diagonal >= 7F ? DeviceType.Tablet : DeviceType.Phone;
            return diagonal >= 5F ? DeviceType.Tablet : DeviceType.Phone;
        }

        private float GetAspectRatio()
        {
            return Mathf.Max((float)Screen.width, Screen.height) / Mathf.Min(Screen.width, Screen.height);
        }

        private float GetDiagonalInches()
        {
            var screenWidth = Screen.width / Screen.dpi;
            var screenHeight = Screen.height / Screen.dpi;
            return Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));
        }

        private DeviceOrientation ToDeviceOrientation(UnityEngine.DeviceOrientation orientation)
        {
            return orientation switch
            {
                UnityEngine.DeviceOrientation.Portrait           => DeviceOrientation.Portrait,
                UnityEngine.DeviceOrientation.PortraitUpsideDown => DeviceOrientation.PortraitUpsideDown,
                UnityEngine.DeviceOrientation.LandscapeLeft      => DeviceOrientation.LandscapeLeft,
                UnityEngine.DeviceOrientation.LandscapeRight     => DeviceOrientation.LandscapeRight,
                UnityEngine.DeviceOrientation.Unknown            => DeviceOrientation.Unknown,
                UnityEngine.DeviceOrientation.FaceUp             => DeviceOrientation.FaceUp,
                UnityEngine.DeviceOrientation.FaceDown           => DeviceOrientation.FaceDown,
                _                                                => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        private DeviceScreenOrientation ToScreenOrientation(ScreenOrientation orientation)
        {
            return orientation switch
            {
                ScreenOrientation.Portrait           => DeviceScreenOrientation.Portrait,
                ScreenOrientation.PortraitUpsideDown => DeviceScreenOrientation.PortraitUpsideDown,
                ScreenOrientation.LandscapeLeft      => DeviceScreenOrientation.LandscapeLeft,
                ScreenOrientation.LandscapeRight     => DeviceScreenOrientation.LandscapeRight,
                _                                    => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        /*
         * Events Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (!paused)
                Check();
        }
    }
}