using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    public sealed class EventMapInfoPool
    {
        private readonly Dictionary<Type, Stack<IEventMapInfo>>   _availableInstances;
        private readonly Dictionary<Type, HashSet<IEventMapInfo>> _usedInstances;

        public EventMapInfoPool()
        {
            _availableInstances = new Dictionary<Type, Stack<IEventMapInfo>>();
            _usedInstances = new Dictionary<Type, HashSet<IEventMapInfo>>();
        }

        internal EventMapInfo             Take()             { return GetInstance<EventMapInfo>(); }
        internal EventMapInfo<T1>         Take<T1>()         { return GetInstance<EventMapInfo<T1>>(); }
        internal EventMapInfo<T1, T2>     Take<T1, T2>()     { return GetInstance<EventMapInfo<T1, T2>>(); }
        internal EventMapInfo<T1, T2, T3> Take<T1, T2, T3>() { return GetInstance<EventMapInfo<T1, T2, T3>>(); }

        internal void Return(IEventMapInfo info)
        {
            var commandType = info.GetType();
            var usedInstances = GetUsedInstances(commandType, false);
            if (usedInstances != null && usedInstances.Remove(info))
                GetAvailableInstances(commandType, false).Push(info);
        }

        /*
         * Private.
         */

        private T GetInstance<T>() where T : IEventMapInfo
        {
            IEventMapInfo info;

            var type = typeof(T);
            var usedInstances = GetUsedInstances(type, true);
            var availableInstances = GetAvailableInstances(type, true);
            if (availableInstances.Count > 0)
            {
                info = availableInstances.Pop();
                usedInstances.Add(info);
            }
            else
            {
                info = (IEventMapInfo)Activator.CreateInstance(type);
                usedInstances.Add(info);
            }
            
            return (T)info;
        }

        private Stack<IEventMapInfo> GetAvailableInstances(Type commandType, bool create)
        {
            if (_availableInstances.TryGetValue(commandType, out var availableInstances) || !create)
                return availableInstances;
            availableInstances = new Stack<IEventMapInfo>();
            _availableInstances.Add(commandType, availableInstances);
            return availableInstances;
        }

        private HashSet<IEventMapInfo> GetUsedInstances(Type commandType, bool create)
        {
            if (_usedInstances.TryGetValue(commandType, out var usedInstances) || !create)
                return usedInstances;
            usedInstances = new HashSet<IEventMapInfo>();
            _usedInstances.Add(commandType, usedInstances);
            return usedInstances;
        }
    }
}