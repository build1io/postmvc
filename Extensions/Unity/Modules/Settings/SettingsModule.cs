using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Settings.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules.Settings
{
    public sealed class SettingsModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }

        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<ISettingsController>().To<SettingsController>().AsSingleton();
        }
    }
}