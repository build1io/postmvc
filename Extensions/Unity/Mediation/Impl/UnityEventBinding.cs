using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using UnityEngine.Events;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    internal sealed class UnityEventBinding : IUnityEventBindingTo, IUnityEventBindingFrom
    {
        private UnityEvent       _unityEvent;
        private IEventDispatcher _dispatcher;

        private List<Event> _events;
        private Action      _action;

        public UnityEventBinding(UnityEvent unityEvent, IEventDispatcher dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);
            _dispatcher = dispatcher;
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

        public void Destroy()
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
}