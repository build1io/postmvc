using System;

namespace Build1.PostMVC.Core.MVCS.Events
{
    public interface IEventMapCore
    {
        /*
         * Map.
         */

        void Map(Event @event, Action listener);
        void Map<T1>(Event<T1> @event, Action<T1> listener);
        void Map<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void Map<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        void Map(IEventDispatcher dispatcher, Event @event, Action listener);
        void Map<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener);
        void Map<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener);
        void Map<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        /*
         * Map Once.
         */

        void MapOnce(Event @event, Action listener);
        void MapOnce<T1>(Event<T1> @event, Action<T1> listener);
        void MapOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void MapOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        void MapOnce(IEventDispatcher dispatcher, Event @event, Action listener);
        void MapOnce<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener);
        void MapOnce<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener);
        void MapOnce<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        /*
         * Unmap.
         */

        void Unmap(Event @event, Action listener);
        void Unmap<T1>(Event<T1> @event, Action<T1> listener);
        void Unmap<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void Unmap<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        void Unmap(IEventDispatcher dispatcher, Event @event, Action listener);
        void Unmap<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener);
        void Unmap<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener);
        void Unmap<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        /*
         * Unmap All.
         */

        void UnmapAll();
        
        /*
         * Dispatch.
         */
        
        void Dispatch(Event @event);
        void Dispatch<T1>(Event<T1> @event, T1 param01);
        void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);
        
        void DispatchLater(Event @event);
        void DispatchLater<T1>(Event<T1> @event, T1 param01);
        void DispatchLater<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void DispatchLater<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);

        /*
         * Map Info.
         */

        bool ContainsMapInfo(Event @event, Action listener);
        bool ContainsMapInfo<T1>(Event<T1> @event, Action<T1> listener);
        bool ContainsMapInfo<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        bool ContainsMapInfo<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        bool ContainsMapInfo(IEventDispatcher dispatcher, Event @event, Action listener);
        bool ContainsMapInfo<T1>(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener);
        bool ContainsMapInfo<T1, T2>(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener);
        bool ContainsMapInfo<T1, T2, T3>(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);
    }
}