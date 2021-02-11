using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;

namespace Build1.PostMVC.Extensions.Unity.Modules.Update.Impl
{
    internal sealed class UpdateController : IUpdateController
    {
        [Inject] public IAgentsController AgentsController { get; set; }

        private UpdateAgent _agent;
        
        [PostConstruct]
        public void PostConstruct()
        {
            _agent = AgentsController.Create<UpdateAgent>();
        }
        
        [PreDestroy]
        public void PreDestroy()
        {
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Public.
         */

        public void SubscribeForFixedUpdate(Action<float> callback)
        {
            _agent.SubscribeForFixedUpdate(callback);
        }

        public void SubscribeForUpdate(Action<float> callback)
        {
            _agent.SubscribeForUpdate(callback);
        }

        public void SubscribeForLateUpdate(Action<float> callback)
        {
            _agent.SubscribeForLateUpdate(callback);
        }

        public void Unsubscribe(Action<float> callback)
        {
            _agent.Unsubscribe(callback);
        }
    }
}