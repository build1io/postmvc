using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Settings;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityToolsModule : Module
    {
        [PostConstruct]
        public void PostConstruct()
        {
            AddModule<SettingsModule>();
        }
    }
}