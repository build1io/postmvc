using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Events
{
    public sealed class EventMap : IEventMap
    {
        private readonly List<EventMapInfo> _infos;
        private readonly IEventDispatcher   _dispatcher;

        public EventMap(IEventDispatcher dispatcher)
        {
            _infos = new List<EventMapInfo>(8);
            _dispatcher = dispatcher;
        }

        /*
         * Map.
         */

        public void Map(Event @event, Action listener)                                     { Map(_dispatcher, @event, listener); }
        public void Map<T1>(Event<T1> @event, Action<T1> listener)                         { Map(_dispatcher, @event, listener); }
        public void Map<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { Map(_dispatcher, @event, listener); }
        public void Map<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { Map(_dispatcher, @event, listener); }

        public void Map(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
        }

        public void Map<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
        }

        public void Map<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
        }

        public void Map<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
        }

        /*
         * Map Once.
         */

        public void MapOnce(Event @event, Action listener)                                     { MapOnce(_dispatcher, @event, listener); }
        public void MapOnce<T1>(Event<T1> @event, Action<T1> listener)                         { MapOnce(_dispatcher, @event, listener); }
        public void MapOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { MapOnce(_dispatcher, @event, listener); }
        public void MapOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { MapOnce(_dispatcher, @event, listener); }

        public void MapOnce(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.AddListenerOnce(@event, listener);
            var info = AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
            dispatcher.AddListenerOnce(@event, () => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.AddListenerOnce(@event, listener);
            var info = AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
            dispatcher.AddListenerOnce(@event, (p1) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.AddListenerOnce(@event, listener);
            var info = AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
            dispatcher.AddListenerOnce(@event, (p1, p2) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.AddListenerOnce(@event, listener);
            var info = AddMapInfo(dispatcher, @event, listener, () => { dispatcher.RemoveListener(@event, listener); });
            dispatcher.AddListenerOnce(@event, (p1, p2, p3) => { RemoveMapInfo(info); });
        }

        /*
         * Unmap.
         */

        public void Unmap(Event @event, Action listener)                                     { Unmap(_dispatcher, @event, listener); }
        public void Unmap<T1>(Event<T1> @event, Action<T1> listener)                         { Unmap(_dispatcher, @event, listener); }
        public void Unmap<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { Unmap(_dispatcher, @event, listener); }
        public void Unmap<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { Unmap(_dispatcher, @event, listener); }

        public void Unmap(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void UnmapAll()
        {
            foreach (var info in _infos)
                info.removeHandler.Invoke();
            _infos.Clear();
        }

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
         * Private.
         */

        private EventMapInfo AddMapInfo(IEventDispatcher dispatcher, EventBase @event, object listener, Action removeHandler)
        {
            var info = new EventMapInfo(dispatcher, @event, listener, removeHandler);
            _infos.Add(info);
            return info;
        }

        private void RemoveMapInfo(EventMapInfo info)
        {
            _infos.Remove(info);
        }

        private void RemoveMapInfo(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            for (var i = _infos.Count - 1; i >= 0; i--)
            {
                var info = _infos[i];
                if (ReferenceEquals(info.dispatcher, dispatcher) && ReferenceEquals(info.@event, @event) && Equals(info.listener, listener))
                    _infos.RemoveAt(i);
            }
        }

        private bool ContainsMapInfoImpl(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            foreach (var info in _infos)
            {
                if (ReferenceEquals(info.dispatcher, dispatcher) && ReferenceEquals(info.@event, @event) && Equals(info.listener, listener))
                    return true;
            }
            return false;
        }
    }
}