using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Core.MVCS.Events.Impl.Bus
{
    internal sealed class EventBusEventInfoPool : Pool<IEventBusEventInfo>
    {
        public     EventBusEventInfo             Take()             { return base.Take<EventBusEventInfo>(); }
        public new EventBusEventInfo<T1>         Take<T1>()         { return base.Take<EventBusEventInfo<T1>>(); }
        public     EventBusEventInfo<T1, T2>     Take<T1, T2>()     { return base.Take<EventBusEventInfo<T1, T2>>(); }
        public     EventBusEventInfo<T1, T2, T3> Take<T1, T2, T3>() { return base.Take<EventBusEventInfo<T1, T2, T3>>(); }
    }
}