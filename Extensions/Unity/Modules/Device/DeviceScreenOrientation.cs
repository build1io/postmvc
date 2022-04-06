using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    [Flags]
    public enum DeviceScreenOrientation
    {
        Portrait           = 1 << 0,
        PortraitUpsideDown = 1 << 1,
        LandscapeLeft      = 1 << 2,
        LandscapeRight     = 1 << 3
    }
}