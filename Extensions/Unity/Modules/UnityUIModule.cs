using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.Device.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Popup;
using Build1.PostMVC.Extensions.Unity.Modules.Popup.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Screen;
using Build1.PostMVC.Extensions.Unity.Modules.Screen.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.UI;
using Build1.PostMVC.Extensions.Unity.Modules.UI.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityUIModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IDeviceController>().To<DeviceController>().AsSingleton();
            InjectionBinder.Bind<IPopupController>().To<PopupController>().AsSingleton();
            InjectionBinder.Bind<IScreensController>().To<ScreensController>().AsSingleton();
            InjectionBinder.Bind<IUILayersController>().To<UILayersController>().AsSingleton();
        }
    }
}