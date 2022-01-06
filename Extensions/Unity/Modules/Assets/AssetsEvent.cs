using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public static class AssetsEvent
    {
        public static readonly Event<Enum, float>     BundleLoadingProgress = new Event<Enum, float>();
        public static readonly Event<AssetBundleInfo> BundleLoadingSuccess  = new Event<AssetBundleInfo>();
        public static readonly Event<Exception>       BundleLoadingFail     = new Event<Exception>();
    }
}