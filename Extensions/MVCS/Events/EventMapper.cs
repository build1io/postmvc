using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Events
{
    public sealed class EventMapper : IEventMapper
    {
        private readonly List<EventMapInfoBase>               _infos;
        private readonly EventDispatcherWithCommandProcessing _dispatcher; // The final type must be specified to escape AOT issues.

        public EventMapper(EventDispatcherWithCommandProcessing dispatcher)
        {
            _infos = new List<EventMapInfoBase>(8);
            _dispatcher = dispatcher;
        }

        /*
         * Map.
         */

        public void Map(Event @event, Action listener)
        {
            _dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1>(Event<T1> @event, Action<T1> listener)
        {
            _dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)
        {
            _dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            _dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        public void Map<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(_dispatcher, @event, listener);
        }

        /*
         * Map Once.
         */

        public void MapOnce(Event @event, Action listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            _dispatcher.AddListenerOnce(@event, listener);
            _dispatcher.AddListenerOnce(@event, () => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1>(Event<T1> @event, Action<T1> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            _dispatcher.AddListenerOnce(@event, listener);
            _dispatcher.AddListenerOnce(@event, (p1) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            _dispatcher.AddListenerOnce(@event, listener);
            _dispatcher.AddListenerOnce(@event, (p1, p2) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            _dispatcher.AddListenerOnce(@event, listener);
            _dispatcher.AddListenerOnce(@event, (p1, p2, p3) => { RemoveMapInfo(info); });
        }

        public void MapOnce(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, () => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, (p1) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, (p1, p2) => { RemoveMapInfo(info); });
        }

        public void MapOnce<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            var info = AddMapInfo(_dispatcher, @event, listener);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, (p1, p2, p3) => { RemoveMapInfo(info); });
        }

        /*
         * Unmap.
         */

        public void Unmap(Event @event, Action listener)
        {
            _dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(_dispatcher, @event, listener);
        }

        public void Unmap<T1>(Event<T1> @event, Action<T1> listener)
        {
            _dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(_dispatcher, @event, listener);
        }

        public void Unmap<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)
        {
            _dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(_dispatcher, @event, listener);
        }

        public void Unmap<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            _dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(_dispatcher, @event, listener);
        }

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
                info.Unbind();
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

        private EventMapInfoBase AddMapInfo(IEventDispatcher dispatcher, Event @event, Action listener)
        {
            var info = new EventMapInfo(dispatcher, @event, listener);
            _infos.Add(info);
            return info;
        }
        
        private EventMapInfoBase AddMapInfo<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            var info = new EventMapInfo<T1>(dispatcher, @event, listener);
            _infos.Add(info);
            return info;
        }
        
        private EventMapInfoBase AddMapInfo<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            var info = new EventMapInfo<T1, T2>(dispatcher, @event, listener);
            _infos.Add(info);
            return info;
        }
        
        private EventMapInfoBase AddMapInfo<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            var info = new EventMapInfo<T1, T2, T3>(dispatcher, @event, listener);
            _infos.Add(info);
            return info;
        }

        private void RemoveMapInfo(EventMapInfoBase info)
        {
            _infos.Remove(info);
        }

        private void RemoveMapInfo(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            for (var i = _infos.Count - 1; i >= 0; i--)
            {
                var info = _infos[i];
                if (info.Match(dispatcher, @event, listener))
                    _infos.RemoveAt(i);
            }
        }

        private bool ContainsMapInfoImpl(IEventDispatcher dispatcher, EventBase @event, object listener)
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