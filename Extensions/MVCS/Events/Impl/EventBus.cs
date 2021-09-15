using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    public sealed class EventBus : IEventBus
    {
        private const int ResolvingCallsCapacity = 16;

        [Inject] public IEventDispatcher Dispatcher { get; set; }

        public bool HasEvents
        {
            get
            {
                lock (_pendingEvents)
                    return _pendingEvents.Count > 0;    
            }
        }

        private readonly List<IEventBusEventInfo> _pendingEvents           = new List<IEventBusEventInfo>(ResolvingCallsCapacity);
        private readonly List<IEventBusEventInfo> _pendingEventsExecutable = new List<IEventBusEventInfo>(ResolvingCallsCapacity);
        private readonly EventBusEventInfoPool    _infosPool               = new EventBusEventInfoPool();
        
        /*
         * Add.
         */
        
        public void Add(Event @event)
        {
            var info = _infosPool.Take();
            info.Setup(Dispatcher, @event);
            
            lock (_pendingEvents)
                _pendingEvents.Add(info);
        }

        public void Add<T1>(Event<T1> @event, T1 param01)
        {
            var info = _infosPool.Take<T1>();
            info.Setup(Dispatcher, @event, param01);
            
            lock (_pendingEvents)
                _pendingEvents.Add(info);
        }

        public void Add<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)
        {
            var info = _infosPool.Take<T1, T2>();
            info.Setup(Dispatcher, @event, param01, param02);
            
            lock (_pendingEvents)
                _pendingEvents.Add(info);
        }
        
        public void Add<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            var info = _infosPool.Take<T1, T2, T3>();
            info.Setup(Dispatcher, @event, param01, param02, param03);
            
            lock (_pendingEvents)
                _pendingEvents.Add(info);
        }

        /*
         * Dispatch.
         */
        
        public void Dispatch()
        {
            lock (_pendingEvents)
            {
                if (_pendingEvents.Count <= 0)
                    return;

                _pendingEventsExecutable.AddRange(_pendingEvents);
                _pendingEvents.Clear();

                foreach (var info in _pendingEventsExecutable)
                {
                    info.Dispatch();
                    _infosPool.Return(info);
                }

                _pendingEventsExecutable.Clear();
            }
        }
    }
}