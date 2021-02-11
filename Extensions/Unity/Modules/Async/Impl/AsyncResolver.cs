using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async.Impl
{
    internal sealed class AsyncResolver : IAsyncResolver
    {
        [Inject] public IAgentsController AgentsController { get; set; }
        
        private AsyncResolverAgent _agent;
        
        [PostConstruct]
        public void PostConstruct()
        {
            _agent = AgentsController.Create<AsyncResolverAgent>();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Actions.
         */
        
        public void Resolve(Action action, bool unique = true)
        {
            _agent.Resolve(action, unique);
        }

        public void Resolve<T>(Action<T> action, T value, bool unique = true)
        {
            _agent.Resolve(action, value, unique);
        }

        /*
         * Delayed Calls.
         */

        public int DelayedCall(Action callback, float seconds)
        {
            return _agent.DelayedCall(callback, seconds);
        }
        
        public int DelayedCall<T>(Action<T> callback, T param, float seconds)
        {
            return _agent.DelayedCall(callback, param, seconds);
        }

        public bool CancelCall(int callId)
        {
            return _agent.CancelCall(callId);
        }
    }
}