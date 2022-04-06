using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Device;
using Build1.PostMVC.Extensions.Unity.Modules.Popups;
using Build1.PostMVC.Extensions.Unity.Modules.Popups.Impl;
using Build1.PostMVC.Extensions.Unity.Modules.Screens;
using Build1.PostMVC.Extensions.Unity.Modules.Screens.Impl;
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
            AddModule<DeviceModule>();
            
            InjectionBinder.Bind<IPopupController>().To<PopupController>().AsSingleton();
            InjectionBinder.Bind<IScreensController>().To<ScreensController>().AsSingleton();
            InjectionBinder.Bind<IUILayersController>().To<UILayersController>().AsSingleton();
        }
    }
}