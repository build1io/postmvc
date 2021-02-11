using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public sealed class PopupConfig : UIControlConfiguration
    {
        public PopupConfig(int assetBundleId, string prefabName, int appLayerId) : base(assetBundleId, prefabName, appLayerId)
        {
        }

        public PopupConfig(DevicePlatform platform, int assetBundleId, string prefabName, int appLayerId) : base(platform, assetBundleId, prefabName, appLayerId)
        {
        }

        public PopupConfig(DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId) : base(deviceType, assetBundleId, prefabName, appLayerId)
        {
        }

        public PopupConfig(DevicePlatform platform, DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId) : base(platform, deviceType, assetBundleId, prefabName, appLayerId)
        {
        }
        
        public new PopupConfig AddBinding<V, M>()
        {
            base.AddBinding<V, M>();
            return this;
        }
        
        public new PopupConfig AddBinding<V, I, M>()
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}