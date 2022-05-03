using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    [Flags]
    public enum AssetBundleInfoSetting
    {
        None               = 0,
        RequestMissingInfo = 1 << 0,
        CacheInfo          = 1 << 1,
        CreateMissingInfo  = 1 << 2
    }
}