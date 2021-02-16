using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.Screen
{
    public sealed class ScreenConfig : UIControlConfiguration
    {
        public ScreenConfig(int assetBundleId, string prefabName, int appLayerId) : base(assetBundleId, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DevicePlatform platform, int assetBundleId, string prefabName, int appLayerId) : base(platform, assetBundleId, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId) : base(deviceType, assetBundleId, prefabName, appLayerId)
        {
        }

        public ScreenConfig(DevicePlatform platform, DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId) : base(platform, deviceType, assetBundleId, prefabName, appLayerId)
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