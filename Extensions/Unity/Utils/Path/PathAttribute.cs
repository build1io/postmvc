using System;

namespace Build1.PostMVC.Extensions.Unity.Utils.Path
{
    [Flags]
    public enum PathAttribute
    {
        Automatic = 0,

        Internal = 1 << 0,
        External = 1 << 1,

        Persistent = 1 << 2,
        Cached     = 1 << 3,

        Absolute  = 1 << 4, // Works the same as Default.
        Canonical = 1 << 5,
        Default   = 1 << 6 // Works the same as Absolute.
    }
}