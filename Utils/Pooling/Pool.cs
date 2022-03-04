using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Extensions.Unity.Modules.Logging;

namespace Build1.PostMVC.Utils.Pooling
{
    public class Pool<T> where T : class
    {
        private readonly ILog log = LogProvider.GetLog<Pool<T>>(LogLevel.None);

        private readonly Dictionary<Type, Stack<T>>   _availableInstances;
        private readonly Dictionary<Type, HashSet<T>> _usedInstances;

        public Pool()
        {
            _availableInstances = new Dictionary<Type, Stack<T>>();
            _usedInstances = new Dictionary<Type, HashSet<T>>();
        }

        /*
         * Counts.
         */

        public int GetAvailableInstancesCount<TT>() where TT : T
        {
            return GetAvailableInstancesCount(typeof(TT));
        }

        public int GetAvailableInstancesCount(Type commandType)
        {
            return GetAvailableInstances(commandType, false)?.Count ?? 0;
        }

        public int GetUsedInstancesCount<TT>() where TT : T
        {
            return GetUsedInstancesCount(typeof(TT));
        }

        public int GetUsedInstancesCount(Type commandType)
        {
            return GetUsedInstances(commandType, false)?.Count ?? 0;
        }

        /*
         * Take.
         */

        public TF Take<TF>() where TF : T, new()
        {
            return Take<TF>(out var isNewInstance);
        }
        
        public TF Take<TF>(out bool isNewInstance) where TF : T, new()
        {
            TF instance;
            var usedInstances = GetUsedInstances(typeof(TF), true);
            var availableInstances = GetAvailableInstances(typeof(TF), true);
            if (availableInstances.Count > 0)
            {
                log.Debug("Taking existing...");

                instance = (TF)availableInstances.Pop();
                usedInstances.Add(instance);

                log.Debug(LogInstances);

                isNewInstance = false;
                return instance;
            }

            log.Debug("Creating instance...");

            instance = Activator.CreateInstance<TF>();
            usedInstances.Add(instance);

            log.Debug(LogInstances);

            isNewInstance = true;
            return instance;
        }

        public T Take(Type instanceType)
        {
            return Take(instanceType, out var inNewInstance);
        }
        
        public T Take(Type instanceType, out bool isNewInstance)
        {
            T instance;
            var usedInstances = GetUsedInstances(instanceType, true);
            var availableInstances = GetAvailableInstances(instanceType, true);
            if (availableInstances.Count > 0)
            {
                log.Debug("Taking existing...");

                instance = availableInstances.Pop();
                usedInstances.Add(instance);

                log.Debug(LogInstances);

                isNewInstance = false;
                return instance;
            }

            log.Debug("Creating instance...");

            instance = (T)Activator.CreateInstance(instanceType);
            usedInstances.Add(instance);

            log.Debug(LogInstances);

            isNewInstance = true;
            return instance;
        }

        /*
         * Instantiate.
         */
        
        public T Instantiate(Type commandType)
        {
            return (T)Activator.CreateInstance(commandType);
        }
        
        /*
         * Return.
         */
        
        public void Return(T instance)
        {
            if (instance == null)
                return;
            
            var commandType = instance.GetType();
            var usedInstances = GetUsedInstances(commandType, false);
            if (usedInstances == null || !usedInstances.Remove(instance))
                return;

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