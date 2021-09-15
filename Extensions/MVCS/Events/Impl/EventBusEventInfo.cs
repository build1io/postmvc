namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventBusEventInfo : IEventBusEventInfo
    {
        private IEventDispatcher _dispatcher;
        private Event            _event;
        
        public void Setup(IEventDispatcher dispatcher, Event @event)
        {
            // To prevent AOT issues.
            _dispatcher = (EventDispatcherWithCommandProcessing)dispatcher;
            _event = @event;
        }
        
        public void Dispatch()
        {
            _dispatcher.Dispatch(_event);
        }
        
        public void OnTake() { }

        public void OnReturn()
        {
            _dispatcher = null;
            _event = null;
        }
    }
    
    internal sealed class EventBusEventInfo<T1> : IEventBusEventInfo
    {
        private IEventDispatcher _dispatcher;
        private Event<T1>            _event;
        private T1               _param01;
        
        public void Setup(IEventDispatcher dispatcher, Event<T1> @event, T1 param01)
        {
            // To prevent AOT issues.
            _dispatcher = (EventDispatcherWithCommandProcessing)dispatcher;
            _event = @event;
            _param01 = param01;
        }
        
        public void Dispatch()
        {
            _dispatcher.Dispatch(_event, _param01);
        }
        
        public void OnTake() { }

        public void OnReturn()
        {
            _dispatcher = null;
            _event = null;
            _param01 = default;
        }
    }
    
    internal sealed class EventBusEventInfo<T1, T2> : IEventBusEventInfo
    {
        private IEventDispatcher _dispatcher;
        private Event<T1, T2>    _event;
        private T1               _param01;
        private T2               _param02;
        
        public void Setup(IEventDispatcher dispatcher, Event<T1, T2> @event, T1 param01, T2 param02)
        {
            // To prevent AOT issues.
            _dispatcher = (EventDispatcherWithCommandProcessing)dispatcher;
            _event = @event;
            _param01 = param01;
            _param02 = param02;
        }
        
        public void Dispatch()
        {
            _dispatcher.Dispatch(_event, _param01, _param02);
        }
        
        public void OnTake() { }

        public void OnReturn()
        {
            _dispatcher = null;
            _event = null;
            _param01 = default;
            _param02 = default;
        }
    }
    
    internal sealed class EventBusEventInfo<T1, T2, T3> : IEventBusEventInfo
    {
        private IEventDispatcher  _dispatcher;
        private Event<T1, T2, T3> _event;
        private T1                _param01;
        private T2                _param02;
        private T3                _param03;
        
        public void Setup(IEventDispatcher dispatcher, Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            // To prevent AOT issues.
            _dispatcher = (EventDispatcherWithCommandProcessing)dispatcher;
            _event = @event;
            _param01 = param01;
            _param02 = param02;
            _param03 = param03;
        }
        
        public void Dispatch()
        {
            _dispatcher.Dispatch(_event, _param01, _param02, _param03);
        }
        
        public void OnTake() { }

        public void OnReturn()
        {
            _dispatcher = null;
            _event = null;
            _param01 = default;
            _param02 = default;
            _param03 = default;
        }
    }
}