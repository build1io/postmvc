using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.InternetReachability;
using Build1.PostMVC.Extensions.Unity.Modules.InternetReachability.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityWebModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IInternetReachabilityController>().To<InternetReachabilityController>().AsSingleton();
        }
    }
}