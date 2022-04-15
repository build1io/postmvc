using System;
using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal partial class EventMap : IEventMap
    {
        private readonly EventDispatcherWithCommandProcessing _dispatcher; // The final type must be specified to escape AOT issues.
        private readonly IEventBus                            _bus;

        private readonly List<IEventMapInfo> _infos;
        private readonly EventMapInfoPool    _infosPool;

        public EventMap(IEventDispatcher dispatcher, IEventBus bus, EventMapInfoPool infosPool)
        {
            _dispatcher = (EventDispatcherWithCommandProcessing)dispatcher;
            _bus = bus;

            _infos = new List<IEventMapInfo>(8);
            _infosPool = infosPool;
        }

        /*
         * Map.
         */

        public void Map(Event @event, Action listener)                                     { AddMapInfo(_dispatcher, @event, listener, false).Bind(); }
        public void Map<T1>(Event<T1> @event, Action<T1> listener)                         { AddMapInfo(_dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(_dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(_dispatcher, @event, listener, false).Bind(); }

        public void Map(IEventDispatcher dispatcher, Event @event, Action listener)                                     { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(dispatcher, @event, listener, false).Bind(); }
        public void Map<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(dispatcher, @event, listener, false).Bind(); }

        /*
         * Map Once.
         */

        public void MapOnce(Event @event, Action listener)                                     { AddMapInfo(_dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1>(Event<T1> @event, Action<T1> listener)                         { AddMapInfo(_dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(_dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(_dispatcher, @event, listener, true).Bind(); }

        public void MapOnce(IEventDispatcher dispatcher, Event @event, Action listener)                                     { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { AddMapInfo(dispatcher, @event, listener, true).Bind(); }
        public void MapOnce<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { AddMapInfo(dispatcher, @event, listener, true).Bind(); }

        /*
         * Unmap.
         */

        public void Unmap(Event @event, Action listener)
        {
            if (TryGetMapInfo(_dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1>(Event<T1> @event, Action<T1> listener)
        {
            if (TryGetMapInfo(_dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)
        {
            if (TryGetMapInfo(_dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            if (TryGetMapInfo(_dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void Unmap<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            if (TryGetMapInfo(dispatcher, @event, listener, out var info))
                RemoveMapInfo(info.Unbind());
        }

        public void UnmapAll()
        {
            foreach (var info in _infos)
            {
                info.Unbind();
                _infosPool.Return(info);
            }

            _infos.Clear();
        }

        /*
         * Dispatch.
         */

        public void Dispatch(Event @event)                                                             { _dispatcher.Dispatch(@event); }
        public void Dispatch<T1>(Event<T1> @event, T1 param01)                                         { _dispatcher.Dispatch(@event, param01); }
        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)                     { _dispatcher.Dispatch(@event, param01, param02); }
        public void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03) { _dispatcher.Dispatch(@event, param01, param02, param03); }

        public void DispatchLater(Event @event)                                                             { _bus.Add(@event); }
        public void DispatchLater<T1>(Event<T1> @event, T1 param01)                                         { _bus.Add(@event, param01); }
        public void DispatchLater<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)                     { _bus.Add(@event, param01, param02); }
        public void DispatchLater<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03) { _bus.Add(@event, param01, param02, param03); }

        /*
         * Map Info.
         */

        public bool ContainsMapInfo(Event @event, Action listener)                                     { return ContainsMapInfoImpl(_dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1>(Event<T1> @event, Action<T1> listener)                         { return ContainsMapInfoImpl(_dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { return ContainsMapInfoImpl(_dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { return ContainsMapInfoImpl(_dispatcher, @event, listener); }

        public bool ContainsMapInfo(IEventDispatcher dispatcher, Event @event, Action listener)                                     { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)                         { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)             { return ContainsMapInfoImpl(dispatcher, @event, listener); }
        public bool ContainsMapInfo<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { return ContainsMapInfoImpl(dispatcher, @event, listener); }

        /*
         * Add Map Info.
         */

        internal EventMapInfo AddMapInfo(IEventDispatcher dispatcher, Event @event, Action listener, bool isOnceScenario)
        {
            var info = _infosPool.Take();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        internal EventMapInfo<T1> AddMapInfo<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener, bool isOnceScenario)
        {
            var info = _infosPool.Take<T1>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        internal EventMapInfo<T1, T2> AddMapInfo<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, bool isOnceScenario)
        {
            var info = _infosPool.Take<T1, T2>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        internal EventMapInfo<T1, T2, T3> AddMapInfo<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, bool isOnceScenario)
        {
            var info = _infosPool.Take<T1, T2, T3>();
            info.Setup(dispatcher, @event, listener, RemoveMapInfo, isOnceScenario);
            _infos.Add(info);
            return info;
        }

        /*
         * Remove Map Info.
         */

        internal void RemoveMapInfo(IEventMapInfo info)
        {
            _infos.Remove(info);
            _infosPool.Return(info);
        }

        protected bool TryGetMapInfo(IEventDispatcher dispatcher, EventBase @event, object listener, out IEventMapInfo info)
        {
            for (var i = _infos.Count - 1; i >= 0; i--)
            {
                info = _infos[i];
                if (info.Match(dispatcher, @event, listener))
                    return true;
            }

            info = null;
            return false;
        }

        /*
         * Other.
         */

        internal bool ContainsMapInfoImpl(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            foreach (var info in _infos)
            {
                if (info.Match(dispatcher, @event, listener))
                    return true;
            }

            return false;
        }
    }
}