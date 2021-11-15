using System;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.App;
using Build1.PostMVC.Extensions.Unity.Modules.Async;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.DeviceOrientation.Impl
{
    internal sealed class DeviceOrientationController : IDeviceOrientationController
    {
        private const float OrientationCheckTimeout = 0.1F;

        [Log(LogLevel.Warning)] public ILog           Log           { get; set; }
        [Inject]                public IEventMap      EventMap      { get; set; }
        [Inject]                public IAsyncResolver AsyncResolver { get; set; }

        public DeviceOrientation Orientation { get; private set; }

        private int               _intervalId;
        private ScreenOrientation _orientation;

        [PostConstruct]
        public void PostConstruct()
        {
            EventMap.Map(AppEvent.Pause, OnAppPause);

            CheckScreenOrientation();
            StartOrientationCheck();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            EventMap.UnmapAll();

            StopOrientationCheck();
        }

        /*
         * Orientations.
         */

        public void SetAvailableOrientations(DeviceOrientation orientations)
        {
            Log.Debug(o => $"SetAvailableOrientations: {o}", orientations);

            switch (orientations)
            {
                case DeviceOrientation.Portrait:
                    Screen.orientation = ScreenOrientation.Portrait;
                    return;
                case DeviceOrientation.PortraitUpsideDown:
                    Screen.orientation = ScreenOrientation.PortraitUpsideDown;
                    return;
                case DeviceOrientation.LandscapeLeft:
                    Screen.orientation = ScreenOrientation.LandscapeLeft;
                    return;
                case DeviceOrientation.LandscapeRight:
                    Screen.orientation = ScreenOrientation.LandscapeRight;
                    return;
            }

            var autorotateToPortrait = (orientations & DeviceOrientation.Portrait) == DeviceOrientation.Portrait;
            var autorotateToPortraitUpsideDown = (orientations & DeviceOrientation.PortraitUpsideDown) == DeviceOrientation.PortraitUpsideDown;
            var autorotateToLandscapeLeft = (orientations & DeviceOrientation.LandscapeLeft) == DeviceOrientation.LandscapeLeft;
            var autorotateToLandscapeRight = (orientations & DeviceOrientation.LandscapeRight) == DeviceOrientation.LandscapeRight;

            Screen.autorotateToPortrait = autorotateToPortrait;
            Screen.autorotateToPortraitUpsideDown = autorotateToPortraitUpsideDown;
            Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
            Screen.autorotateToLandscapeRight = autorotateToLandscapeRight;
            Screen.orientation = ScreenOrientation.AutoRotation;
        }

        /*
         * Private.
         */

        private void CheckScreenOrientation()
        {
            if (_orientation == Screen.orientation)
                return;

            Log.Debug(o => $"CheckScreenOrientation: {o}", Screen.orientation);

            _orientation = Screen.orientation;
            Orientation = ToDeviceOrientation(_orientation);

            EventMap.Dispatch(DeviceOrientationEvent.Changed, Orientation);
        }

        private void StartOrientationCheck()
        {
            Log.Debug("StartOrientationCheck");

            if (_intervalId == AsyncResolver.DefaultCallId)
                _intervalId = AsyncResolver.IntervalCall(CheckScreenOrientation, OrientationCheckTimeout);
        }

        private void StopOrientationCheck()
        {
            Log.Debug("StopOrientationCheck");

            AsyncResolver.CancelCall(ref _intervalId);
        }

        private DeviceOrientation ToDeviceOrientation(ScreenOrientation orientation)
        {
            return orientation switch
            {
                ScreenOrientation.Portrait           => DeviceOrientation.Portrait,
                ScreenOrientation.PortraitUpsideDown => DeviceOrientation.PortraitUpsideDown,
                ScreenOrientation.LandscapeLeft      => DeviceOrientation.LandscapeLeft,
                ScreenOrientation.LandscapeRight     => DeviceOrientation.LandscapeRight,
                _                                    => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        /*
         * Events Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (!paused)
                CheckScreenOrientation();
        }
    }
}