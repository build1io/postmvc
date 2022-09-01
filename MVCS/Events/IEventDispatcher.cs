using System;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Events
{
    public interface IEventDispatcher
    {
        void AddListener(Event @event, Action listener);
        void AddListener<T1>(Event<T1> @event, Action<T1> listener);
        void AddListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void AddListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        void AddListenerOnce(Event @event, Action listener);
        void AddListenerOnce<T1>(Event<T1> @event, Action<T1> listener);
        void AddListenerOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void AddListenerOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);
        
        bool ContainsListener(Event @event, Action listener);
        bool ContainsListener<T1>(Event<T1> @event, Action<T1> listener);
        bool ContainsListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        bool ContainsListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);
        
        void RemoveListener(Event @event, Action listener);
        void RemoveListener<T1>(Event<T1> @event, Action<T1> listener);
        void RemoveListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener);
        void RemoveListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener);

        void RemoveAllListeners();
        void RemoveAllListeners(EventBase @event);

        void Dispatch(Event @event);
        void Dispatch<T1>(Event<T1> @event, T1 param01);
        void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);
    }
}