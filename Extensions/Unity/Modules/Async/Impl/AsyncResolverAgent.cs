using System;
using System.Collections;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using Random = System.Random;

namespace Build1.PostMVC.Extensions.Unity.Modules.Async.Impl
{
    public class AsyncResolverAgent : MonoBehaviour
    {
        [Logger(LogLevel.Verbose)] public ILogger Logger { get; set; }

        private const int PendingActionsCapacity = 8;
        private const int PendingCallsCapacity   = 8;

        private readonly List<Action> _pendingActions           = new List<Action>(PendingActionsCapacity);
        private readonly List<Action> _pendingActionsExecutable = new List<Action>(PendingActionsCapacity);

        private readonly List<int> _pendingCallIds = new List<int>(PendingCallsCapacity);

        private readonly Random _random = new Random();

        [PreDestroy]
        public void PreDestroy()
        {
            // Removed callbacks ids and no callbacks will be called.
            // Still it's not the proper disposing as coroutines will be running.
            // Not sure if it'll be an issue as current game object will be removed from the scene and all coroutines will be stopped.
            _pendingCallIds.Clear();
        }

        /*
         * Actions.
         */

        public void Resolve(Action action, bool unique)
        {
            lock (_pendingActions)
            {
                if (unique && _pendingActions.IndexOf(action) != -1)
                    return;
                _pendingActions.Add(action);
                if (_pendingActions.Count > PendingActionsCapacity)
                    Logger.Warn(() => $"Estimated capacity of {PendingActionsCapacity} for pending actions exceeded. Increase the capacity to save memory.");
            }
        }

        public void Resolve<T>(Action<T> action, T value, bool unique)
        {
            Resolve(() => action.Invoke(value), unique);
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
         * Delayed Call.
         */

        public int DelayedCall(Action callback, float seconds)
        {
            var callId = GenerateCallId();
            _pendingCallIds.Add(callId);
            StartCoroutine(DelayedCallImpl(callId, seconds, callback));
            return callId;
        }

        public int DelayedCall<T>(Action<T> callback, T param, float seconds)
        {
            var callId = GenerateCallId();
            _pendingCallIds.Add(callId);
            StartCoroutine(DelayedCallImpl(callId, seconds, callback, param));
            return callId;
        }

        public bool CancelCall(int callId)
        {
            return _pendingCallIds.Remove(callId);
        }

        IEnumerator DelayedCallImpl(int callId, float seconds, Action callback)
        {
            yield return new WaitForSeconds(seconds);
            if (_pendingCallIds.Remove(callId))
                callback.Invoke();
        }

        IEnumerator DelayedCallImpl<T>(int callId, float seconds, Action<T> callback, T param)
        {
            yield return new WaitForSeconds(seconds);
            if (_pendingCallIds.Remove(callId))
                callback.Invoke(param);
        }

        private int GenerateCallId()
        {
            int callId;
            do
            {
                callId = _random.Next(int.MaxValue);
            } while (_pendingCallIds.Contains(callId));

            return callId;
        }
    }
}