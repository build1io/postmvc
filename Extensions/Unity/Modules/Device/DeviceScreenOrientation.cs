using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    [Flags]
    public enum DeviceScreenOrientation
    {
        Unknown            = 1 << 0,
        Portrait           = 1 << 1,
        PortraitUpsideDown = 1 << 2,
        LandscapeLeft      = 1 << 3,
        LandscapeRight     = 1 << 4
    }
}