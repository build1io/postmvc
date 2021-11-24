using System;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.App;
using Build1.PostMVC.Extensions.Unity.Modules.Async;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Orientation.Impl
{
    internal sealed class OrientationController : IOrientationController
    {
        private const float OrientationCheckTimeout = 0.1F;

        [Log(LogLevel.Warning)] public ILog           Log           { get; set; }
        [Inject]                public IEventMap      EventMap      { get; set; }
        [Inject]                public IAsyncResolver AsyncResolver { get; set; }

        public DeviceOrientation DeviceOrientation { get; private set; }
        public ScreenOrientation ScreenOrientation { get; private set; }

        private int                           _intervalId;
        private UnityEngine.DeviceOrientation _deviceOrientation;
        private UnityEngine.ScreenOrientation _screenOrientation;

        [PostConstruct]
        public void PostConstruct()
        {
            EventMap.Map(AppEvent.Pause, OnAppPause);

            CheckOrientation();
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

        public void SetAvailableOrientations(ScreenOrientation orientations)
        {
            Log.Debug(o => $"SetAvailableOrientations: {o}", orientations);

            switch (orientations)
            {
                case ScreenOrientation.Portrait:
                    Screen.orientation = UnityEngine.ScreenOrientation.Portrait;
                    return;
                case ScreenOrientation.PortraitUpsideDown:
                    Screen.orientation = UnityEngine.ScreenOrientation.PortraitUpsideDown;
                    return;
                case ScreenOrientation.LandscapeLeft:
                    Screen.orientation = UnityEngine.ScreenOrientation.LandscapeLeft;
                    return;
                case ScreenOrientation.LandscapeRight:
                    Screen.orientation = UnityEngine.ScreenOrientation.LandscapeRight;
                    return;
            }

            var autorotateToPortrait = (orientations & ScreenOrientation.Portrait) == ScreenOrientation.Portrait;
            var autorotateToPortraitUpsideDown = (orientations & ScreenOrientation.PortraitUpsideDown) == ScreenOrientation.PortraitUpsideDown;
            var autorotateToLandscapeLeft = (orientations & ScreenOrientation.LandscapeLeft) == ScreenOrientation.LandscapeLeft;
            var autorotateToLandscapeRight = (orientations & ScreenOrientation.LandscapeRight) == ScreenOrientation.LandscapeRight;

            Screen.autorotateToPortrait = autorotateToPortrait;
            Screen.autorotateToPortraitUpsideDown = autorotateToPortraitUpsideDown;
            Screen.autorotateToLandscapeLeft = autorotateToLandscapeLeft;
            Screen.autorotateToLandscapeRight = autorotateToLandscapeRight;
            Screen.orientation = UnityEngine.ScreenOrientation.AutoRotation;
        }

        /*
         * Private.
         */

        private void CheckOrientation()
        {
            if (CheckDeviceOrientationValid(Input.deviceOrientation) && _deviceOrientation != Input.deviceOrientation)
            {
                Log.Debug(o => $"CheckOrientation: for Device {o}", Input.deviceOrientation);

                _deviceOrientation = Input.deviceOrientation;
                DeviceOrientation = ToDeviceOrientation(Input.deviceOrientation);

                EventMap.Dispatch(OrientationEvent.DeviceOrientationChanged, DeviceOrientation);
            }

            if (_screenOrientation != Screen.orientation)
            {
                Log.Debug(o => $"CheckOrientation: for Screen {o}", Screen.orientation);

                _screenOrientation = Screen.orientation;
                ScreenOrientation = ToScreenOrientation(_screenOrientation);

                EventMap.Dispatch(OrientationEvent.ScreenOrientationChanged, ScreenOrientation);    
            }
        }

        private void StartOrientationCheck()
        {
            Log.Debug("StartOrientationCheck");

            if (_intervalId == AsyncResolver.DefaultCallId)
                _intervalId = AsyncResolver.IntervalCall(CheckOrientation, OrientationCheckTimeout);
        }

        private void StopOrientationCheck()
        {
            Log.Debug("StopOrientationCheck");

            AsyncResolver.CancelCall(ref _intervalId);
        }
        
        private DeviceOrientation ToDeviceOrientation(UnityEngine.DeviceOrientation orientation)
        {
            return orientation switch
            {
                UnityEngine.DeviceOrientation.Portrait           => DeviceOrientation.Portrait,
                UnityEngine.DeviceOrientation.PortraitUpsideDown => DeviceOrientation.PortraitUpsideDown,
                UnityEngine.DeviceOrientation.LandscapeLeft      => DeviceOrientation.LandscapeLeft,
                UnityEngine.DeviceOrientation.LandscapeRight     => DeviceOrientation.LandscapeRight,
                _                                                => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        private ScreenOrientation ToScreenOrientation(UnityEngine.ScreenOrientation orientation)
        {
            return orientation switch
            {
                UnityEngine.ScreenOrientation.Portrait           => ScreenOrientation.Portrait,
                UnityEngine.ScreenOrientation.PortraitUpsideDown => ScreenOrientation.PortraitUpsideDown,
                UnityEngine.ScreenOrientation.LandscapeLeft      => ScreenOrientation.LandscapeLeft,
                UnityEngine.ScreenOrientation.LandscapeRight     => ScreenOrientation.LandscapeRight,
                _                                                => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, null)
            };
        }

        private bool CheckDeviceOrientationValid(UnityEngine.DeviceOrientation orientation)
        {
            return orientation != UnityEngine.DeviceOrientation.Unknown && 
                   orientation != UnityEngine.DeviceOrientation.FaceDown &&
                   orientation != UnityEngine.DeviceOrientation.FaceUp;
        }
        
        /*
         * Events Handlers.
         */

        private void OnAppPause(bool paused)
        {
            if (!paused)
                CheckOrientation();
        }
    }
}