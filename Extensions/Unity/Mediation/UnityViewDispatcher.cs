using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.Unity.Mediation.Impl;
using UnityEngine.Events;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public abstract class UnityViewDispatcher : UnityView, IEventDispatcher
    {
        private readonly IEventDispatcher                                  _dispatcher;
        private readonly Dictionary<UnityEventBase, UnityEventBindingBase> _bindings;

        protected UnityViewDispatcher()
        {
            _dispatcher = new EventDispatcher();
            _bindings = new Dictionary<UnityEventBase, UnityEventBindingBase>();
        }

        /*
         * Event Dispatcher.
         */

        public void AddListener(Event @event, Action listener)                                     { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListener(@event, listener); }

        public void AddListenerOnce(Event @event, Action listener)                                     { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListenerOnce(@event, listener); }

        public bool ContainsListener(Event @event, Action listener)                                     { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1>(Event<T1> @event, Action<T1> listener)                         { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { return _dispatcher.ContainsListener(@event, listener); }
        public bool ContainsListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { return _dispatcher.ContainsListener(@event, listener); }

        public void RemoveListener(Event @event, Action listener)                                     { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.RemoveListener(@event, listener); }

        public void RemoveAllListeners()                 { _dispatcher.RemoveAllListeners(); }
        public void RemoveAllListeners(EventBase @event) { _dispatcher.RemoveAllListeners(@event); }

        public void Dispatch(Event @event)                                                             { _dispatcher.Dispatch(@event); }
        public void Dispatch<T1>(Event<T1> @event, T1 param01)                                         { _dispatcher.Dispatch(@event, param01); }
        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)                     { _dispatcher.Dispatch(@event, param01, param02); }
        public void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03) { _dispatcher.Dispatch(@event, param01, param02, param03); }

        /*
         * Event Dispatcher Extension.
         */

        protected IUnityEventBindingTo BindUnityEvent(UnityEvent unityEvent)
        {
            if (_bindings.TryGetValue(unityEvent, out var binding))
                return (IUnityEventBindingTo)binding;
            binding = new UnityEventBinding(unityEvent, _dispatcher);
            _bindings.Add(unityEvent, binding);
            return (IUnityEventBindingTo)binding;
        }

        protected IUnityEventBindingTo<T1> BindUnityEvent<T1>(UnityEvent<T1> unityEvent)
        {
            if (_bindings.TryGetValue(unityEvent, out var binding))
                return (IUnityEventBindingTo<T1>)binding;
            binding = new UnityEventBinding<T1>(unityEvent, _dispatcher);
            _bindings.Add(unityEvent, binding);
            return (IUnityEventBindingTo<T1>)binding;
        }

        protected IUnityEventBindingTo<T1, T2> BindUnityEvent<T1, T2>(UnityEvent<T1, T2> unityEvent)
        {
            if (_bindings.TryGetValue(unityEvent, out var binding))
                return (IUnityEventBindingTo<T1, T2>)binding;
            binding = new UnityEventBinding<T1, T2>(unityEvent, _dispatcher);
            _bindings.Add(unityEvent, binding);
            return (IUnityEventBindingTo<T1, T2>)binding;
        }

        protected IUnityEventBindingTo<T1, T2, T3> BindUnityEvent<T1, T2, T3>(UnityEvent<T1, T2, T3> unityEvent)
        {
            if (_bindings.TryGetValue(unityEvent, out var binding))
                return (IUnityEventBindingTo<T1, T2, T3>)binding;
            binding = new UnityEventBinding<T1, T2, T3>(unityEvent, _dispatcher);
            _bindings.Add(unityEvent, binding);
            return (IUnityEventBindingTo<T1, T2, T3>)binding;
        }

        protected IUnityEventBindingFrom             UnbindUnityEvent(UnityEvent unityEvent)                         { return (IUnityEventBindingFrom)_bindings[unityEvent]; }
        protected IUnityEventBindingFrom<T1>         UnbindUnityEvent<T1>(UnityEvent<T1> unityEvent)                 { return (IUnityEventBindingFrom<T1>)_bindings[unityEvent]; }
        protected IUnityEventBindingFrom<T1, T2>     UnbindUnityEvent<T1, T2>(UnityEvent<T1, T2> unityEvent)         { return (IUnityEventBindingFrom<T1, T2>)_bindings[unityEvent]; }
        protected IUnityEventBindingFrom<T1, T2, T3> UnbindUnityEvent<T1, T2, T3>(UnityEvent<T1, T2, T3> unityEvent) { return (IUnityEventBindingFrom<T1, T2, T3>)_bindings[unityEvent]; }

        protected void UnbindUnityEventCompletely(UnityEventBase unityEvent)
        {
            if (!_bindings.TryGetValue(unityEvent, out var binding))
                return;
            binding.Destroy();
            _bindings.Remove(unityEvent);
        }

        protected void UnbindAllUnityEvents()
        {
            foreach (var bridge in _bindings.Values)
                bridge.Destroy();
            _bindings.Clear();
        }
    }
}