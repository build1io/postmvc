using System;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async
{
    public interface IAsyncResolver
    {
        /*
         * Actions.
         */

        void Resolve(Action action, bool unique = true);
        void Resolve<T>(Action<T> action, T value, bool unique = true);

        /*
         * Delayed Calls.
         */

        int  DelayedCall(Action callback, float seconds);
        int  DelayedCall<T>(Action<T> callback, T param, float seconds);
        bool CancelCall(int callId);
    }
}