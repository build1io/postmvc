using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using UnityEngine.Events;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    internal sealed class UnityEventBinding : UnityEventBindingBase, IUnityEventBindingTo, IUnityEventBindingFrom
    {
        private UnityEvent  _unityEvent;
        private List<Event> _events;
        private Action      _action;

        public UnityEventBinding(UnityEvent unityEvent, IEventDispatcher dispatcher) : base(dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);
        }

        public IUnityEventBindingTo ToEvent(Event @event)
        {
            if (_events == null)
                _events = new List<Event> { @event };
            else
                _events.Add(@event);
            return this;
        }

        public IUnityEventBindingTo ToAction(Action action)
        {
            _action += action;
            return this;
        }

        public IUnityEventBindingFrom FromEvent(Event @event)
        {
            _events?.Remove(@event);
            return this;
        }

        public IUnityEventBindingFrom FromAction(Action action)
        {
            _action -= action;
            return this;
        }

        public override void Destroy()
        {
            _unityEvent?.RemoveListener(EventHandler);
            _unityEvent = null;
            _dispatcher = null;
        }

        /*
         * Event Handlers.
         */

        private void EventHandler()
        {
            if (_events != null)
            {
                foreach (var @event in _events)
                    _dispatcher.Dispatch(@event);
            }

            _action?.Invoke();
        }
    }

    internal sealed class UnityEventBinding<T1> : UnityEventBindingBase, IUnityEventBindingTo<T1>, IUnityEventBindingFrom<T1>
    {
        private UnityEvent<T1>  _unityEvent;
        private List<Event<T1>> _events;
        private Action<T1>      _action;

        public UnityEventBinding(UnityEvent<T1> unityEvent, IEventDispatcher dispatcher) : base(dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);
        }
        
        public IUnityEventBindingTo<T1> ToEvent(Event<T1> @event)
        {
            if (_events == null)
                _events = new List<Event<T1>> { @event };
            else
                _events.Add(@event);
            return this;
        }
        
        public IUnityEventBindingTo<T1> ToAction(Action<T1> action)
        {
            _action += action;
            return this;
        }
        
        public IUnityEventBindingFrom<T1> FromEvent(Event<T1> @event)
        {
            _events?.Remove(@event);
            return this;
        }

        public IUnityEventBindingFrom<T1> FromAction(Action<T1> action)
        {
            _action -= action;
            return this;
        }
        
        public override void Destroy()
        {
            _unityEvent?.RemoveListener(EventHandler);
            _unityEvent = null;
            _dispatcher = null;
        }
        
        /*
         * Event Handlers.
         */

        private void EventHandler(T1 param)
        {
            if (_events != null)
            {
                foreach (var @event in _events)
                    _dispatcher.Dispatch(@event, param);
            }

            _action?.Invoke(param);
        }
    }
    
    internal sealed class UnityEventBinding<T1, T2> : UnityEventBindingBase, IUnityEventBindingTo<T1, T2>, IUnityEventBindingFrom<T1, T2>
    {
        private UnityEvent<T1, T2>  _unityEvent;
        private List<Event<T1, T2>> _events;
        private Action<T1, T2>      _action;

        public UnityEventBinding(UnityEvent<T1, T2> unityEvent, IEventDispatcher dispatcher) : base(dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);
        }
        
        public IUnityEventBindingTo<T1, T2> ToEvent(Event<T1, T2> @event)
        {
            if (_events == null)
                _events = new List<Event<T1, T2>> { @event };
            else
                _events.Add(@event);
            return this;
        }
        
        public IUnityEventBindingTo<T1, T2> ToAction(Action<T1, T2> action)
        {
            _action += action;
            return this;
        }
        
        public IUnityEventBindingFrom<T1, T2> FromEvent(Event<T1, T2> @event)
        {
            _events?.Remove(@event);
            return this;
        }

        public IUnityEventBindingFrom<T1, T2> FromAction(Action<T1, T2> action)
        {
            _action -= action;
            return this;
        }
        
        public override void Destroy()
        {
            _unityEvent?.RemoveListener(EventHandler);
            _unityEvent = null;
            _dispatcher = null;
        }
        
        /*
         * Event Handlers.
         */

        private void EventHandler(T1 param01, T2 param02)
        {
            if (_events != null)
            {
                foreach (var @event in _events)
                    _dispatcher.Dispatch(@event, param01, param02);
            }

            _action?.Invoke(param01, param02);
        }
    }
    
    internal sealed class UnityEventBinding<T1, T2, T3> : UnityEventBindingBase, IUnityEventBindingTo<T1, T2, T3>, IUnityEventBindingFrom<T1, T2, T3>
    {
        private UnityEvent<T1, T2, T3>  _unityEvent;
        private List<Event<T1, T2, T3>> _events;
        private Action<T1, T2, T3>      _action;

        public UnityEventBinding(UnityEvent<T1, T2, T3> unityEvent, IEventDispatcher dispatcher) : base(dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);
        }
        
        public IUnityEventBindingTo<T1, T2, T3> ToEvent(Event<T1, T2, T3> @event)
        {
            if (_events == null)
                _events = new List<Event<T1, T2, T3>> { @event };
            else
                _events.Add(@event);
            return this;
        }
        
        public IUnityEventBindingTo<T1, T2, T3> ToAction(Action<T1, T2, T3> action)
        {
            _action += action;
            return this;
        }
        
        public IUnityEventBindingFrom<T1, T2, T3> FromEvent(Event<T1, T2, T3> @event)
        {
            _events?.Remove(@event);
            return this;
        }

        public IUnityEventBindingFrom<T1, T2, T3> FromAction(Action<T1, T2, T3> action)
        {
            _action -= action;
            return this;
        }
        
        public override void Destroy()
        {
            _unityEvent?.RemoveListener(EventHandler);
            _unityEvent = null;
            _dispatcher = null;
        }
        
        /*
         * Event Handlers.
         */

        private void EventHandler(T1 param01, T2 param02, T3 param03)
        {
            if (_events != null)
            {
                foreach (var @event in _events)
                    _dispatcher.Dispatch(@event, param01, param02, param03);
            }

            _action?.Invoke(param01, param02, param03);
        }
    }
}