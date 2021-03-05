using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public sealed class ScreenConfig : UIControlConfiguration
    {
        public ScreenConfig(AssetBundle assetBundle, string prefabName, int appLayerId) : base(assetBundle, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DevicePlatform platform, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, assetBundle, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(deviceType, assetBundle, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : base(platform, deviceType, assetBundle, prefabName, appLayerId)
        {
        }

        public new ScreenConfig AddBinding<V, M>() where V : UnityView
                                                   where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new ScreenConfig AddBinding<V, I, M>() where V : UnityView, I
                                                      where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}