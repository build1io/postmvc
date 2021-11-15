using System;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public sealed class PopupConfig : UIControlConfiguration
    {
        public PopupConfig(AssetBundle assetBundle, string prefabName, int appLayerId) : base(assetBundle, prefabName, appLayerId) { }
        public PopupConfig(Enum assetBundleId, string prefabName, int appLayerId) : base(assetBundleId, prefabName, appLayerId) { }

        public PopupConfig(DevicePlatform platform, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, assetBundle, prefabName, appLayerId) { }
        public PopupConfig(DevicePlatform platform, Enum assetBundleId, string prefabName, int appLayerId) : base(platform, assetBundleId, prefabName, appLayerId) { }

        public PopupConfig(DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(deviceType, assetBundle, prefabName, appLayerId) { }
        public PopupConfig(DeviceType deviceType, Enum assetBundleId, string prefabName, int appLayerId) : base(deviceType, assetBundleId, prefabName, appLayerId) { }

        public PopupConfig(DevicePlatform platform, DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, deviceType, assetBundle, prefabName, appLayerId) { }
        public PopupConfig(DevicePlatform platform, DeviceType deviceType, Enum assetBundleId, string prefabName, int appLayerId) : base(platform, deviceType, assetBundleId, prefabName, appLayerId) { }

        public new PopupConfig AddBinding<V, M>() where V : IPopupView
                                                  where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new PopupConfig AddBinding<V, I, M>() where V : IPopupView, I
                                                     where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}