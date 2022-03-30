using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Modules.Assets
{
    public static class AssetsEvent
    {
        public static readonly Event<AssetBundleInfo>            BundleLoadingProgress = new Event<AssetBundleInfo>();
        public static readonly Event<AssetBundleInfo>            BundleLoadingSuccess  = new Event<AssetBundleInfo>();
        public static readonly Event<AssetBundleInfo, Exception> BundleLoadingFail     = new Event<AssetBundleInfo, Exception>();
    }
}