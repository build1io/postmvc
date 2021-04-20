using System;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapInfo : EventMapInfoBase
    {
        public readonly Event  @event;
        public readonly Action listener;

        public EventMapInfo(IEventDispatcher dispatcher, Event @event, Action listener) : base(dispatcher)
        {
            this.@event = @event;
            this.listener = listener;
        }

        public override void Unbind()
        {
            switch (dispatcher)
            {
                case EventDispatcherWithCommandProcessing processing:
                    processing.RemoveListener(@event, listener);
                    break;
                case EventDispatcher eventDispatcher:
                    eventDispatcher.RemoveListener(@event, listener);
                    break;
                default:
                    dispatcher.RemoveListener(@event, listener);
                    break;
            }
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(this.dispatcher, dispatcher) &&
                   ReferenceEquals(this.@event, @event) &&
                   Equals(this.listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1> : EventMapInfoBase
    {
        public readonly Event<T1>  @event;
        public readonly Action<T1> listener;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1> @event, Action<T1> listener) : base(dispatcher)
        {
            this.@event = @event;
            this.listener = listener;
        }

        public override void Unbind()
        {
            switch (dispatcher)
            {
                case EventDispatcherWithCommandProcessing processing:
                    processing.RemoveListener(@event, listener);
                    break;
                case EventDispatcher eventDispatcher:
                    eventDispatcher.RemoveListener(@event, listener);
                    break;
                default:
                    dispatcher.RemoveListener(@event, listener);
                    break;
            }
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(this.dispatcher, dispatcher) &&
                   ReferenceEquals(this.@event, @event) &&
                   Equals(this.listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2> : EventMapInfoBase
    {
        public readonly Event<T1, T2>  @event;
        public readonly Action<T1, T2> listener;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener) : base(dispatcher)
        {
            this.@event = @event;
            this.listener = listener;
        }

        public override void Unbind()
        {
            switch (dispatcher)
            {
                case EventDispatcherWithCommandProcessing processing:
                    processing.RemoveListener(@event, listener);
                    break;
                case EventDispatcher eventDispatcher:
                    eventDispatcher.RemoveListener(@event, listener);
                    break;
                default:
                    dispatcher.RemoveListener(@event, listener);
                    break;
            }
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(this.dispatcher, dispatcher) &&
                   ReferenceEquals(this.@event, @event) &&
                   Equals(this.listener, listener);
        }
    }

    internal sealed class EventMapInfo<T1, T2, T3> : EventMapInfoBase
    {
        public readonly Event<T1, T2, T3>  @event;
        public readonly Action<T1, T2, T3> listener;

        public EventMapInfo(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) : base(dispatcher)
        {
            this.@event = @event;
            this.listener = listener;
        }

        public override void Unbind()
        {
            switch (dispatcher)
            {
                case EventDispatcherWithCommandProcessing processing:
                    processing.RemoveListener(@event, listener);
                    break;
                case EventDispatcher eventDispatcher:
                    eventDispatcher.RemoveListener(@event, listener);
                    break;
                default:
                    dispatcher.RemoveListener(@event, listener);
                    break;
            }
        }

        public override bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(this.dispatcher, dispatcher) &&
                   ReferenceEquals(this.@event, @event) &&
                   Equals(this.listener, listener);
        }
    }
}