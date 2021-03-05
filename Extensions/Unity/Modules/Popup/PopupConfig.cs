using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Popup
{
    public sealed class PopupConfig : UIControlConfiguration
    {
        public PopupConfig(AssetBundle assetBundle, string prefabName, int appLayerId) : base(assetBundle, prefabName, appLayerId)
        {
        }
        
        public PopupConfig(DevicePlatform platform, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, assetBundle, prefabName, appLayerId)
        {
        }

        public PopupConfig(DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(deviceType, assetBundle, prefabName, appLayerId)
        {
        }

        public PopupConfig(DevicePlatform platform, DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, deviceType, assetBundle, prefabName, appLayerId)
        {
        }

        public new PopupConfig AddBinding<V, M>() where V : UnityView
                                                  where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new PopupConfig AddBinding<V, I, M>() where V : UnityView, I
                                                     where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}