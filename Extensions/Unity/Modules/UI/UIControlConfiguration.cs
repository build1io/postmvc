using System.Collections;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.Unity.Modules.Assets;
using Build1.PostMVC.Extensions.Unity.Modules.Device;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    public abstract class UIControlConfiguration : IDeviceDependentConfiguration, IEnumerable<UIControlBinding>
    {
        public DevicePlatform DevicePlatform { get; }
        public DeviceType     DeviceType     { get; }

        public readonly AssetBundle            assetBundle;
        public readonly string                 prefabName;
        public readonly int                    appLayerId;
        public readonly List<UIControlBinding> bindings;

        protected UIControlConfiguration(AssetBundle assetBundle, string prefabName, int appLayerId) : this(DevicePlatform.Any, DeviceType.Any, assetBundle, prefabName, appLayerId)
        {
        }

        protected UIControlConfiguration(DevicePlatform platform, AssetBundle assetBundle, string prefabName, int appLayerId) : this(platform, DeviceType.Any, assetBundle, prefabName, appLayerId)
        {
        }

        protected UIControlConfiguration(DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId) : this(DevicePlatform.Any, deviceType, assetBundle, prefabName, appLayerId)
        {
        }

        protected UIControlConfiguration(DevicePlatform platform, DeviceType deviceType, AssetBundle assetBundle, string prefabName, int appLayerId)
        {
            DevicePlatform = platform;
            DeviceType = deviceType;

            this.assetBundle = assetBundle;
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