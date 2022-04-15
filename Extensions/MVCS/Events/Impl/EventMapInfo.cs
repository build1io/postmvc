using System;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapInfo : IEventMapInfo
    {
        private IEventDispatcher      _dispatcher;
        private Event                 _event;
        private Action                _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(IEventDispatcher dispatcher, Event @event, Action listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }

        public void OnceListener()
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Unbind()
        {
            _dispatcher.RemoveListener(_event, _listener);
            
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1> : IEventMapInfo
    {
        private IEventDispatcher      _dispatcher;
        private Event<T1>             _event;
        private Action<T1>            _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }
        
        public void OnceListener(T1 param01)
        {
            _infoRemovalMethod.Invoke(this);
        }
        
        public void Unbind()
        {
            _dispatcher.RemoveListener(_event, _listener);
            
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2> : IEventMapInfo
    {
        private IEventDispatcher      _dispatcher;
        private Event<T1, T2>         _event;
        private Action<T1, T2>        _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }
        
        public void OnceListener(T1 param01, T2 param02)
        {
            _infoRemovalMethod.Invoke(this);
        }
        
        public void Unbind()
        {
            _dispatcher.RemoveListener(_event, _listener);
            
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2, T3> : IEventMapInfo
    {
        private IEventDispatcher      _dispatcher;
        private Event<T1, T2, T3>     _event;
        private Action<T1, T2, T3>    _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }
        
        public void OnceListener(T1 param01, T2 param02, T3 param03)
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Unbind()
        {
            _dispatcher.RemoveListener(_event, _listener);
            
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }
}