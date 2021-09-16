using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;

namespace Build1.PostMVC.Utils.Pooling
{
    internal abstract class Pool<T> where T : IPoolable
    {
        private readonly ILog log = LogProvider.GetLog<Pool<T>>(LogLevel.None);

        private readonly Dictionary<Type, Stack<T>>   _availableInstances;
        private readonly Dictionary<Type, HashSet<T>> _usedInstances;

        protected Pool()
        {
            _availableInstances = new Dictionary<Type, Stack<T>>();
            _usedInstances = new Dictionary<Type, HashSet<T>>();
        }

        /*
         * Public.
         */

        public TF Take<TF>() where TF : T, new()
        {
            var usedInstances = GetUsedInstances(typeof(TF), true);
            var availableInstances = GetAvailableInstances(typeof(TF), true);
            if (availableInstances.Count > 0)
            {
                log.Debug("Taking existing...");

                var info = availableInstances.Pop();
                usedInstances.Add(info);

                log.Debug(LogInstances);

                info.OnTake();
                return (TF)info;
            }

            log.Debug("Creating instance...");

            var instance = Activator.CreateInstance<TF>();
            usedInstances.Add(instance);

            log.Debug(LogInstances);

            instance.OnTake();
            return instance;
        }

        public void Return(T instance)
        {
            var commandType = instance.GetType();
            var usedInstances = GetUsedInstances(commandType, false);
            if (usedInstances == null || !usedInstances.Remove(instance))
                return;

            instance.OnReturn();
            GetAvailableInstances(commandType, false).Push(instance);

            log.Debug("Returning instance...");
            log.Debug(LogInstances);
        }

        /*
         * Private.
         */

        private Stack<T> GetAvailableInstances(Type type, bool create)
        {
            if (_availableInstances.TryGetValue(type, out var availableInstances) || !create)
                return availableInstances;
            availableInstances = new Stack<T>();
            _availableInstances.Add(type, availableInstances);
            return availableInstances;
        }

        private HashSet<T> GetUsedInstances(Type type, bool create)
        {
            if (_usedInstances.TryGetValue(type, out var usedInstances) || !create)
                return usedInstances;
            usedInstances = new HashSet<T>();
            _usedInstances.Add(type, usedInstances);
            return usedInstances;
        }

        private void LogInstances(ILogDebug log)
        {
            var used = _usedInstances.Values.Sum(instances => instances.Count);
            var available = _availableInstances.Values.Sum(instances => instances.Count);

            log.Debug($"Used: {used} Available: {available}");
        }
    }
}