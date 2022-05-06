using System;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popups
{
    public abstract class PopupConfigBase : UIControlConfiguration
    {
        protected PopupConfigBase(AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(bundleInfo, prefabName, appLayerId) { }
        protected PopupConfigBase(Enum bundleId, string prefabName, int appLayerId) : base(bundleId, prefabName, appLayerId) { }

        protected PopupConfigBase(DevicePlatform platform, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(platform, bundleInfo, prefabName, appLayerId) { }
        protected PopupConfigBase(DevicePlatform platform, Enum bundleId, string prefabName, int appLayerId) : base(platform, bundleId, prefabName, appLayerId) { }

        protected PopupConfigBase(DeviceType deviceType, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(deviceType, bundleInfo, prefabName, appLayerId) { }
        protected PopupConfigBase(DeviceType deviceType, Enum bundleId, string prefabName, int appLayerId) : base(deviceType, bundleId, prefabName, appLayerId) { }

        protected PopupConfigBase(DevicePlatform platform, DeviceType deviceType, AssetBundleInfo bundleInfo, string prefabName, int appLayerId) : base(platform, deviceType, bundleInfo, prefabName, appLayerId) { }
        protected PopupConfigBase(DevicePlatform platform, DeviceType deviceType, Enum bundleId, string prefabName, int appLayerId) : base(platform, deviceType, bundleId, prefabName, appLayerId) { }

        public new PopupConfigBase AddBinding<V, M>() where V : IUnityView
                                                      where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new PopupConfigBase AddBinding<V, I, M>() where V : IUnityView, I
                                                         where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}