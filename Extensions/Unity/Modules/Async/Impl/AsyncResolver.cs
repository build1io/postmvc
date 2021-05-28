using System;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Agents;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async.Impl
{
    internal sealed class AsyncResolver : IAsyncResolver
    {
        [Inject] public IAgentsController AgentsController { get; set; }

        public int DefaultCallId => 0;
        
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
         * Resolve.
         */

        public void Resolve(Action action, bool unique = true)                { _agent.Resolve(action, unique); }
        public void Resolve<T>(Action<T> action, T value, bool unique = true) { _agent.Resolve(action, value, unique); }

        /*
         * Calls.
         */

        public int DelayedCall(Action callback, float seconds)                { return _agent.DelayedCall(callback, seconds); }
        public int DelayedCall<T>(Action<T> callback, T param, float seconds) { return _agent.DelayedCall(callback, param, seconds); }

        public int IntervalCall(Action callback, float seconds)                { return _agent.IntervalCall(callback, seconds); }
        public int IntervalCall<T>(Action<T> callback, T param, float seconds) { return _agent.IntervalCall(callback, param, seconds); }

        public bool CancelCall(int callId)
        {
            return _agent.CancelCall(callId);
        }

        public bool CancelCall(ref int callId)
        {
            var removed = _agent.CancelCall(callId);
            if (removed)
                callId = DefaultCallId;
            return removed;
        }
    }
}