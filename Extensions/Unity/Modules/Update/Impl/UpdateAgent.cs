using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;
using Logger = Build1.PostMVC.Extensions.Unity.Modules.Logging.Logger;
using ILogger = Build1.PostMVC.Extensions.Unity.Modules.Logging.ILogger;

namespace Build1.PostMVC.Extensions.Unity.Modules.Update.Impl
{
    public sealed class UpdateAgent : MonoBehaviour
    {
        private const int FixedUpdatesCapacity = 4;
        private const int UpdatesCapacity      = 4;
        private const int LateUpdatesCapacity  = 4;

        [Logger(LogLevel.Verbose)] public ILogger Logger { get; set; }

        private readonly List<Action<float>> _updates          = new List<Action<float>>(UpdatesCapacity);
        private readonly List<Action<float>> _updatesFixed     = new List<Action<float>>(FixedUpdatesCapacity);
        private readonly List<Action<float>> _updatesLate      = new List<Action<float>>(LateUpdatesCapacity);
        private readonly List<Action<float>> _updatesExecuting = new List<Action<float>>(UpdatesCapacity);

        /*
         * Public.
         */

        public void SubscribeForFixedUpdate(Action<float> callback)
        {
            if (_updatesFixed.Contains(callback))
            {
                Logger.Warn(() => "Already subscribed for FixedUpdate.");
                return;
            }

            _updatesFixed.Add(callback);
            if (_updatesFixed.Count > FixedUpdatesCapacity)
                Logger.Warn(() => $"Estimated capacity of FixedUpdates ({FixedUpdatesCapacity}) exceeded. Increase the capacity to use memory efficiently.");
        }

        public void SubscribeForUpdate(Action<float> callback)
        {
            if (_updates.Contains(callback))
            {
                Logger.Warn(() => "Already subscribed for Update.");
                return;
            }

            _updates.Add(callback);
            if (_updates.Count > UpdatesCapacity)
                Logger.Warn(() => $"Estimated capacity of Updates ({UpdatesCapacity}) exceeded. Increase the capacity to use memory efficiently.");
        }

        public void SubscribeForLateUpdate(Action<float> callback)
        {
            if (_updatesLate.Contains(callback))
            {
                Logger.Warn(() => "Already subscribed for Update.");
                return;
            }

            _updatesLate.Add(callback);
            if (_updatesLate.Count > LateUpdatesCapacity)
                Logger.Warn(() => $"Estimated capacity of LateUpdates ({LateUpdatesCapacity}) exceeded. Increase the capacity to use memory efficiently.");
        }

        public void Unsubscribe(Action<float> callback)
        {
            if (_updates.Remove(callback))
                return;
            
            if (_updatesFixed.Remove(callback))
                return;
            
            _updatesLate.Remove(callback);
        }

        /*
         * Update.
         */

        private void FixedUpdate() { TryExecute(_updatesFixed); }
        private void Update()      { TryExecute(_updates); }
        private void LateUpdate()  { TryExecute(_updatesLate); }

        /*
         * Private.
         */

        private void TryExecute(IReadOnlyCollection<Action<float>> callbacks)
        {
            if (callbacks.Count == 0)
                return;

            _updatesExecuting.AddRange(callbacks);

            foreach (var callback in _updatesExecuting)
                callback.Invoke(Time.deltaTime);

            _updatesExecuting.Clear();
        }
    }
}