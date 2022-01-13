using System;
using System.Threading.Tasks;
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

            AsyncResolverStaticAdapter.AsyncResolver = this;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            AgentsController.Destroy(ref _agent);
        }

        /*
         * Resolve.
         */

        public void Resolve(Action action, bool unique = true)                                         { _agent.Resolve(action, unique); }
        public void Resolve<T1>(Action<T1> action, T1 value, bool unique = true)                       { _agent.Resolve(action, value, unique); }
        public void Resolve<T1, T2>(Action<T1, T2> action, T1 value01, T2 value02, bool unique = true) { _agent.Resolve(action, value01, value02, unique); }

        /*
         * Resolve Tasks.
         */

        public void ResolveTask(Task task, Action<Task> continuation)
        {
            task.ContinueWith(innerTask =>
            {
                Resolve(continuation.Invoke, innerTask);
            });
        }

        public void ResolveTask<T>(Task<T> task, Action<Task<T>> continuation)
        {
            task.ContinueWith(innerTask =>
            {
                Resolve(continuation.Invoke, innerTask);
            });
        }
        
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