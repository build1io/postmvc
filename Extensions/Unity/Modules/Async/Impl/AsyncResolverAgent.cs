using System;
using System.Collections;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using Random = System.Random;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async.Impl
{
    public class AsyncResolverAgent : MonoBehaviour
    {
        [Log(LogLevel.Warning)] public ILog Log { get; set; }

        private const int ResolvingCallsCapacity = 8;
        private const int PendingCallsCapacity   = 8;
        private const int IntervalCallsCapacity  = 8;

        private readonly List<Action> _pendingActions           = new List<Action>(ResolvingCallsCapacity);
        private readonly List<Action> _pendingActionsExecutable = new List<Action>(ResolvingCallsCapacity);

        private readonly List<int> _delayedCallIds  = new List<int>(PendingCallsCapacity);
        private readonly List<int> _intervalCallIds = new List<int>(IntervalCallsCapacity);

        private readonly Random _random = new Random();

        [PreDestroy]
        public void PreDestroy()
        {
            // Removed callbacks ids and no callbacks will be called.
            // Still it's not the proper disposing as coroutines will be running.
            // Not sure if it'll be an issue as current game object will be removed from the scene and all coroutines will be stopped.
            _delayedCallIds.Clear();
        }

        /*
         * Update.
         */

        private void Update()
        {
            lock (_pendingActions)
            {
                if (_pendingActions.Count <= 0)
                    return;

                _pendingActionsExecutable.AddRange(_pendingActions);
                _pendingActions.Clear();

                foreach (var action in _pendingActionsExecutable)
                    action();

                _pendingActionsExecutable.Clear();
            }
        }

        /*
         * Resolve.
         */

        public void Resolve(Action action, bool unique)
        {
            lock (_pendingActions)
            {
                if (unique && _pendingActions.IndexOf(action) != -1)
                    return;
                _pendingActions.Add(action);
                if (_pendingActions.Count > ResolvingCallsCapacity)
                    Log.Warn(c => $"Estimated capacity of {c} for pending actions exceeded. Increase the capacity to save memory.", ResolvingCallsCapacity);
            }
        }

        public void Resolve<T1>(Action<T1> action, T1 value, bool unique)
        {
            Resolve(() => action.Invoke(value), unique);
        }
        
        public void Resolve<T1, T2>(Action<T1, T2> action, T1 value01, T2 value02, bool unique)
        {
            Resolve(() => action.Invoke(value01, value02), unique);
        }

        /*
         * Calls.
         */

        public int DelayedCall(Action callback, float seconds)
        {
            var callId = GenerateCallId();
            _delayedCallIds.Add(callId);
            StartCoroutine(DelayedCallImpl(callId, seconds, callback));
            return callId;
        }

        public int DelayedCall<T>(Action<T> callback, T param, float seconds)
        {
            var callId = GenerateCallId();
            _delayedCallIds.Add(callId);
            StartCoroutine(DelayedCallImpl(callId, seconds, callback, param));
            return callId;
        }

        public int IntervalCall(Action callback, float seconds)
        {
            var callId = GenerateCallId();
            _intervalCallIds.Add(callId);
            StartCoroutine(IntervalCallImpl(callId, seconds, callback));
            return callId;
        }

        public int IntervalCall<T>(Action<T> callback, T param, float seconds)
        {
            var callId = GenerateCallId();
            _intervalCallIds.Add(callId);
            StartCoroutine(IntervalCallImpl(callId, seconds, callback, param));
            return callId;
        }

        public bool CancelCall(int callId)
        {
            return _delayedCallIds.Remove(callId) || 
                   _intervalCallIds.Remove(callId);
        }
        
        /*
         * Coroutines.
         */

        IEnumerator DelayedCallImpl(int callId, float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            if (_delayedCallIds.Remove(callId))
                callback.Invoke();
        }

        IEnumerator DelayedCallImpl<T>(int callId, float seconds, Action<T> callback, T param)
        {
            yield return new WaitForSeconds(seconds);
            if (_delayedCallIds.Remove(callId))
                callback.Invoke(param);
        }

        IEnumerator IntervalCallImpl(int callId, float seconds, Action callback)
        {
            while (_intervalCallIds.Contains(callId))
            {
                yield return new WaitForSeconds(seconds);
                callback.Invoke();
            }
        }
        
        IEnumerator IntervalCallImpl<T>(int callId, float seconds, Action<T> callback, T param)
        {
            while (_intervalCallIds.Contains(callId))
            {
                yield return new WaitForSeconds(seconds);
                callback.Invoke(param);
            }
        }
        
        /*
         * Helpers.
         */

        private int GenerateCallId()
        {
            int callId;
            do
            {
                callId = _random.Next(1, int.MaxValue);
            } while (_delayedCallIds.Contains(callId) || _intervalCallIds.Contains(callId));

            return callId;
        }
    }
}