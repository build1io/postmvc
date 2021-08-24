namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMap : EventMapper, IEventMap
    {
        public EventMap(EventDispatcherWithCommandProcessing dispatcher) : base(dispatcher)
        {
        }

        public void Dispatch(Event @event)                                                             { _dispatcher.Dispatch(@event); }
        public void Dispatch<T1>(Event<T1> @event, T1 param01)                                         { _dispatcher.Dispatch(@event, param01); }
        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)                     { _dispatcher.Dispatch(@event, param01, param02); }
        public void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03) { _dispatcher.Dispatch(@event, param01, param02, param03); }
    }
}