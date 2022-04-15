using System;
using Build1.PostMVC.Extensions.Unity.Mediation;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal partial class EventMap
    {
        /*
         * Map.
         */

        public void Map(UnityViewDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, false);
        }

        public void Map<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, false);
        }

        public void Map<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, false);
        }

        public void Map<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.AddListener(@event, listener);
            AddMapInfo(dispatcher, @event, listener, false);
        }

        /*
         * Map Once.
         */

        public void MapOnce(UnityViewDispatcher dispatcher, Event @event, Action listener)
        {
            var info = AddMapInfo(dispatcher, @event, listener, true);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, info.OnceListener);
        }

        public void MapOnce<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            var info = AddMapInfo(dispatcher, @event, listener, true);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, info.OnceListener);
        }

        public void MapOnce<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            var info = AddMapInfo(dispatcher, @event, listener, true);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, info.OnceListener);
        }

        public void MapOnce<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            var info = AddMapInfo(dispatcher, @event, listener, true);
            dispatcher.AddListenerOnce(@event, listener);
            dispatcher.AddListenerOnce(@event, info.OnceListener);
        }

        /*
         * Unmap.
         */

        public void Unmap(UnityViewDispatcher dispatcher, Event @event, Action listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1>(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1, T2>(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
        }

        public void Unmap<T1, T2, T3>(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener)
        {
            dispatcher.RemoveListener(@event, listener);
            RemoveMapInfo(dispatcher, @event, listener);
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