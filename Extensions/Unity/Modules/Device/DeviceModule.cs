using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Device.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules.Device
{
    public sealed class DeviceModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            #if UNITY_EDITOR
                InjectionBinder.Bind<IDeviceController>().To<DeviceControllerEditor>().AsSingleton().ConstructOnStart();
            #else
                InjectionBinder.Bind<IDeviceController>().To<DeviceControllerRuntime>().AsSingleton().ConstructOnStart();
            #endif
        }
    }
}