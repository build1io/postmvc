using System;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Events.Mapping.Impl
{
    internal sealed class EventMapInfo : EventMapInfoBase
    {
        private readonly Event                 _event;
        private readonly Action                _listener;
        private readonly Action<Event, Action> _unbindHandler;

        public EventMapInfo(IEventDispatcher dispatcher, Event @event, Action listener, Action<Event, Action> unbindHandler) : base(dispatcher)
        {
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public override void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1> : EventMapInfoBase
    {
        private readonly Event<T1>                     _event;
        private readonly Action<T1>                    _listener;
        private readonly Action<Event<T1>, Action<T1>> _unbindHandler;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener, Action<Event<T1>, Action<T1>> unbindHandler) : base(dispatcher)
        {
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public override void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2> : EventMapInfoBase
    {
        private readonly Event<T1, T2>                         _event;
        private readonly Action<T1, T2>                        _listener;
        private readonly Action<Event<T1, T2>, Action<T1, T2>> _unbindHandler;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, Action<Event<T1, T2>, Action<T1, T2>> unbindHandler) : base(dispatcher)
        {
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public override void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2, T3> : EventMapInfoBase
    {
        private readonly Event<T1, T2, T3>                             _event;
        private readonly Action<T1, T2, T3>                            _listener;
        private readonly Action<Event<T1, T2, T3>, Action<T1, T2, T3>> _unbindHandler;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, Action<Event<T1, T2, T3>, Action<T1, T2, T3>> unbindHandler) : base(dispatcher)
        {
            _event = @event;
            _listener = listener;
            _unbindHandler = unbindHandler;
        }

        public override void Unbind()
        {
            _unbindHandler.Invoke(_event, _listener);
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }
}