using System;
using System.Threading.Tasks;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async
{
    public interface IAsyncResolver
    {
        int DefaultCallId { get; }

        /*
         * Resolve.
         */

        void Resolve(Action action, bool unique = true);
        void Resolve<T1>(Action<T1> action, T1 value, bool unique = true);
        void Resolve<T1, T2>(Action<T1, T2> action, T1 value01, T2 value02, bool unique = true);

        /*
         * Resolve Tasks.
         */

        void ResolveTask(Task task, Action<Task> continuation);
        void ResolveTask<T>(Task<T> task, Action<Task<T>> continuation);

        /*
         * Calls.
         */

        int DelayedCall(Action callback, float seconds);
        int DelayedCall<T>(Action<T> callback, T param, float seconds);

        int IntervalCall(Action callback, float seconds);
        int IntervalCall<T>(Action<T> callback, T param, float seconds);

        bool CancelCall(int callId);
        bool CancelCall(ref int callId);
    }
}