using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Notifications;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityMobileModule : Module
    {
        [PostConstruct]
        public void PostConstruct()
        {
            AddModule<NotificationsModule>();
        }
    }
}