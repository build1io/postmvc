using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.Unity.Mediation.Api;
using UnityEngine.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation.Impl
{
    internal sealed class UnityEventBinding : IUnityEventBindingTo, IUnityEventBindingFrom
    {
        private UnityEvent       _unityEvent;
        private IEventDispatcher _dispatcher;

        private List<Event>  _events;
        private List<Action> _actions;

        public UnityEventBinding(UnityEvent unityEvent, IEventDispatcher dispatcher)
        {
            _unityEvent = unityEvent;
            _unityEvent.AddListener(EventHandler);

            _dispatcher = dispatcher;

            _events = new List<Event>();
            _actions = new List<Action>();
        }

        public IUnityEventBindingTo ToEvent(Event @event)
        {
            _events.Add(@event);
            return this;
        }

        public IUnityEventBindingTo ToAction(Action action)
        {
            _actions.Add(action);
            return this;
        }

        public IUnityEventBindingFrom FromEvent(Event @event)
        {
            _events.Remove(@event);
            return this;
        }
        
        public IUnityEventBindingFrom FromAction(Action action) 
        { 
            _actions.Remove(action);
            return this;
        }

        public void Destroy()
        {
            if (_unityEvent != null)
            {
                _unityEvent.RemoveListener(EventHandler);
                _unityEvent = null;
            }

            _dispatcher = null;
            _events = null;
            _actions = null;
        }

        /*
         * Event Handlers.
         */

        private void EventHandler()
        {
            foreach (var @event in _events)
                _dispatcher.Dispatch(@event);

            foreach (var action in _actions)
                action.Invoke();
        }
    }
}