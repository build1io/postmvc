using System;
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
    }
}