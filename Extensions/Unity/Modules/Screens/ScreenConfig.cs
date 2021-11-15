using System;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screens
{
    public sealed class ScreenConfig : UIControlConfiguration
    {
        public ScreenConfig(AssetBundle assetBundle, string prefabName, int appLayerId) : base(assetBundle, prefabName, appLayerId) { }
        public ScreenConfig(Enum assetBundleId, string prefabName, int appLayerId) : base(assetBundleId, prefabName, appLayerId) { }

        public ScreenConfig(DevicePlatform platform, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, assetBundle, prefabName, appLayerId) { }
        public ScreenConfig(DevicePlatform platform, Enum assetBundleId, string prefabName, int appLayerId) : base(platform, assetBundleId, prefabName, appLayerId) { }

        public ScreenConfig(DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(deviceType, assetBundle, prefabName, appLayerId) { }
        public ScreenConfig(DeviceType deviceType, Enum assetBundleId, string prefabName, int appLayerId) : base(deviceType, assetBundleId, prefabName, appLayerId) { }

        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, deviceType, assetBundle, prefabName, appLayerId) { }
        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, Enum assetBundleId, string prefabName, int appLayerId) : base(platform, deviceType, assetBundleId, prefabName, appLayerId) { }

        public new ScreenConfig AddBinding<V, M>() where V : IUnityView
                                                   where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new ScreenConfig AddBinding<V, I, M>() where V : IUnityView, I
                                                      where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}