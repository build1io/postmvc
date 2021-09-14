using System;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapInfo : IEventMapInfo
    {
        private IEventDispatcher      _dispatcher;
        private Event                 _event;
        private Action                _listener;
        private Action<Event, Action> _unbindHandler;

        public void Setup(IEventDispatcher dispatcher, Event @event, Action listener, Action<Event, Action> unbindHandler)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
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
        private IEventDispatcher              _dispatcher;
        private Event<T1>                     _event;
        private Action<T1>                    _listener;
        private Action<Event<T1>, Action<T1>> _unbindHandler;

        public void Setup(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener, Action<Event<T1>, Action<T1>> unbindHandler)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
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
        private IEventDispatcher                      _dispatcher;
        private Event<T1, T2>                         _event;
        private Action<T1, T2>                        _listener;
        private Action<Event<T1, T2>, Action<T1, T2>> _unbindHandler;

        public void Setup(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, Action<Event<T1, T2>, Action<T1, T2>> unbindHandler)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
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
        private IEventDispatcher                              _dispatcher;
        private Event<T1, T2, T3>                             _event;
        private Action<T1, T2, T3>                            _listener;
        private Action<Event<T1, T2, T3>, Action<T1, T2, T3>> _unbindHandler;

        public void Setup(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, Action<Event<T1, T2, T3>, Action<T1, T2, T3>> unbindHandler)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }
}