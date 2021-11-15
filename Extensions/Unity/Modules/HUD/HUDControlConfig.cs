using System;
using Build1.PostMVC.Extensions.MVCS.Mediation;
using Build1.PostMVC.Extensions.Unity.Mediation;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.UI;

namespace Build1.PostMVC.Extensions.Unity.Modules.HUD
{
    public sealed class HUDControlConfig : UIControlConfiguration
    {
        public HUDControlConfig(Enum assetBundleId, 
                                string prefabName, 
                                int appLayerId) : base(assetBundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DevicePlatform platform, 
                                Enum assetBundleId, 
                                string prefabName, 
                                int appLayerId) : base(platform, assetBundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DeviceType deviceType, 
                                Enum assetBundleId, 
                                string prefabName, 
                                int appLayerId) : base(deviceType, assetBundleId, prefabName, appLayerId) { }
        
        public HUDControlConfig(DevicePlatform platform, 
                                DeviceType deviceType, 
                                Enum assetBundleId, 
                                string prefabName, 
                                int appLayerId) : base(platform, deviceType, assetBundleId, prefabName, appLayerId) { }

        public new HUDControlConfig AddBinding<V, M>() where V : IUnityView
                                                       where M : Mediator
        {
            base.AddBinding<V, M>();
            return this;
        }

        public new HUDControlConfig AddBinding<V, I, M>() where V : IUnityView, I
                                                          where M : Mediator
        {
            base.AddBinding<V, I, M>();
            return this;
        }
    }
}