using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;
using UnityEngine;

namespace Build1.PostMVC.Extensions.Unity.Modules.Update.Impl
{
    public sealed class UpdateAgent : MonoBehaviour
    {
        private const int FixedUpdatesCapacity = 4;
        private const int UpdatesCapacity      = 4;
        private const int LateUpdatesCapacity  = 4;

        [Log(LogLevel.Warning)] public ILog Log { get; set; }

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
                Log.Warn("Already subscribed for FixedUpdate.");
                return;
            }

            _updatesFixed.Add(callback);
            if (_updatesFixed.Count > FixedUpdatesCapacity)
                Log.Warn(c => $"Estimated capacity of FixedUpdates ({c}) exceeded. Increase the capacity to use memory efficiently.", FixedUpdatesCapacity);
        }

        public void SubscribeForUpdate(Action<float> callback)
        {
            if (_updates.Contains(callback))
            {
                Log.Warn("Already subscribed for Update.");
                return;
            }

            _updates.Add(callback);
            if (_updates.Count > UpdatesCapacity)
                Log.Warn(c => $"Estimated capacity of Updates ({c}) exceeded. Increase the capacity to use memory efficiently.", UpdatesCapacity);
        }

        public void SubscribeForLateUpdate(Action<float> callback)
        {
            if (_updatesLate.Contains(callback))
            {
                Log.Warn("Already subscribed for Update.");
                return;
            }

            _updatesLate.Add(callback);
            if (_updatesLate.Count > LateUpdatesCapacity)
                Log.Warn(c => $"Estimated capacity of LateUpdates ({c}) exceeded. Increase the capacity to use memory efficiently.", LateUpdatesCapacity);
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