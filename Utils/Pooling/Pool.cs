using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Core.Utils.Pooling
{
    public class Pool<T> where T : class
    {
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
                instance = (TF)availableInstances.Pop();
                usedInstances.Add(instance);

                isNewInstance = false;
                return instance;
            }

            instance = Activator.CreateInstance<TF>();
            usedInstances.Add(instance);

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
                instance = availableInstances.Pop();
                usedInstances.Add(instance);

                isNewInstance = false;
                return instance;
            }

            instance = (T)Activator.CreateInstance(instanceType);
            usedInstances.Add(instance);

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
    }
}