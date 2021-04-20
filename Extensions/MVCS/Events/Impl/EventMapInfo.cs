using System;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapInfo
    {
        public readonly IEventDispatcher dispatcher;
        public readonly EventBase        @event;
        public readonly object           listener;
        public readonly Action           removeHandler;

        public EventMapInfo(IEventDispatcher dispatcher, EventBase @event, object listener, Action removeHandler)
        {
            this.dispatcher = dispatcher;
            this.@event = @event;
            this.listener = listener;
            this.removeHandler = removeHandler;
        }
    }
}