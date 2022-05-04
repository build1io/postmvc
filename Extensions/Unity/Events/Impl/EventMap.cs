using System;
using Build1.PostMVC.Extensions.Unity.Events.Impl;
using Build1.PostMVC.Extensions.Unity.Mediation;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal partial class EventMap
    {
        /*
         * Map.
         */

        public void Map(UnityViewDispatcher dispatcher, Event @event, Action listener)                                     { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(dispatcher, @event, listener, false).Bind(); }

        /*
         * Map Once.
         */

        public void MapOnce(UnityViewDispatcher dispatcher, Event @event, Action listener)                                     { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(dispatcher, @event, listener, true).Bind(); }

        /*
         * Unmap.
         */

        public void Unmap(UnityViewDispatcher dispatcher, Event @event, Action listener)
        { 
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        /*
         * Map Info.
         */

        public bool ContainsMapInfo(UnityViewDispatcher dispatcher, Event @event, Action listener)                                     { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        
        /*
         * Add Map Info by UnityViewDispatcher.
         */
        
        private EventMapInfoUnityViewDispatcher AddMapInfo(UnityViewDispatcher dispatcher, Event @event, Action listener, bool isOnceScenario)
        {
            var info = _infosPool.TakeUnityViewDispatcher();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        private EventMapInfoUnityViewDispatcher<T1> AddMapInfo<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener, bool isOnceScenario)
        {
            var info = _infosPool.TakeUnityViewDispatcher<T1>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        private EventMapInfoUnityViewDispatcher<T1, T2> AddMapInfo<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, bool isOnceScenario)
        {
            var info = _infosPool.TakeUnityViewDispatcher<T1, T2>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        private EventMapInfoUnityViewDispatcher<T1, T2, T3> AddMapInfo<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, bool isOnceScenario)
        {
            var info = _infosPool.TakeUnityViewDispatcher<T1, T2, T3>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }
    }
}