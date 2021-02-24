using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.FullScreen;
using Build1.PostMVC.Extensions.Unity.Modules.FullScreen.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityWebGLModule : Module
    {
        [Inject] public IInjectionBinder InjectionBinder { get; set; }
        
        [PostConstruct]
        public void PostConstruct()
        {
            InjectionBinder.Bind<IFullScreenController>().To<FullScreenController>().AsSingleton();
        }
    }
}