using System.Collections;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.Unity.Modules.Device;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    public class UIControlConfiguration : IDeviceDependentConfiguration, IEnumerable<UIControlBinding>
    {
        public DevicePlatform DevicePlatform { get; }
        public DeviceType     DeviceType     { get; }

        public readonly int                    assetBundleId;
        public readonly string                 prefabName;
        public readonly int                    appLayerId;
        public readonly List<UIControlBinding> bindings;

        public UIControlConfiguration(int assetBundleId, string prefabName, int appLayerId) : this(DevicePlatform.Any, DeviceType.Any, assetBundleId, prefabName, appLayerId)
        {
        }
        
        public UIControlConfiguration(DevicePlatform platform, int assetBundleId, string prefabName, int appLayerId) : this(platform, DeviceType.Any, assetBundleId, prefabName, appLayerId)
        {
        }
        
        public UIControlConfiguration(DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId) : this(DevicePlatform.Any, deviceType, assetBundleId, prefabName, appLayerId)
        {
        }
        
        public UIControlConfiguration(DevicePlatform platform, DeviceType deviceType, int assetBundleId, string prefabName, int appLayerId)
        {
            DevicePlatform = platform;
            DeviceType = deviceType;

            this.assetBundleId = assetBundleId;
            this.prefabName = prefabName;
            this.appLayerId = appLayerId;
            
            bindings = new List<UIControlBinding>();
        }

        protected UIControlConfiguration AddBinding<V>()
        {
            bindings.Add(new UIControlBinding(typeof(V)));
            return this;
        }

        protected UIControlConfiguration AddBinding<V, M>()
        {
            bindings.Add(new UIControlBinding(typeof(V), typeof(M)));
            return this;
        }
        
        protected UIControlConfiguration AddBinding<V, I, M>()
        {
            bindings.Add(new UIControlBinding(typeof(V), typeof(I), typeof(M)));
            return this;
        }
        
        public IEnumerator<UIControlBinding> GetEnumerator()
        {
            return bindings.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}