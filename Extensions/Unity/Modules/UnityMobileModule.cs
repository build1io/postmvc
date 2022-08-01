using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;
using Build1.PostMVC.Extensions.Unity.Modules.Mobile;
using Build1.PostMVC.Extensions.Unity.Modules.Mobile.Impl;
using Build1.PostMVC.Modules;

namespace Build1.PostMVC.Extensions.Unity.Modules
{
    public sealed class UnityMobileModule : Module
    {
        [Inject] public IAgentsController AgentsController { get; set; }
        [Inject] public IEventDispatcher  Dispatcher       { get; set; }

        private MobileAgent _mobileAgent;

        [PostConstruct]
        public void PostConstruct()
        {
            _mobileAgent = AgentsController.Create<MobileAgent>();
            _mobileAgent.OnEsc += OnEsc;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            _mobileAgent.OnEsc -= OnEsc;
            AgentsController.Destroy(ref _mobileAgent);
        }

        /*
         * Event Handlers.
         */

        private void OnEsc()
        {
            Dispatcher.Dispatch(AndroidEvent.Back);
        }
    }
}