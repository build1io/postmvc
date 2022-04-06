using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    [Flags]
    public enum DevicePlatform
    {
        Unknown = 0,
        Any     = 1 << 0,

        iOS     = 1 << 1,
        Android = 1 << 2,

        WebGL   = 1 << 3,
        OSX     = 1 << 4,
        Windows = 1 << 5,
        Linux   = 1 << 6,

        XboxOne = 1 << 7,
        PS4     = 1 << 8,
        PS5     = 1 << 9,
        Switch  = 1 << 10,
        tvOS    = 1 << 11
    }
}