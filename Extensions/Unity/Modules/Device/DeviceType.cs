using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    [Flags]
    public enum DeviceType
    {
        Unknown = 0,
        Any     = 1 << 0,
        Phone   = 1 << 1,
        Tablet  = 1 << 2,
        Desktop = 1 << 3,
        Console = 1 << 4,
        TV      = 1 << 5
    }
}