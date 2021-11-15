using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.InternetReachability.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules.InternetReachability
{
    public sealed class InternetReachabilityModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IInternetReachabilityController>().To<InternetReachabilityController>().AsSingleton();
        }
    }
}