using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Core.MVCS.Events.Impl.Map
{
    public class EventMapInfoPool : Pool<IEventMapInfo>
    {
        public EventMapInfoInterface             TakeInterface()             { return Take<EventMapInfoInterface>(); }
        public EventMapInfoInterface<T1>         TakeInterface<T1>()         { return Take<EventMapInfoInterface<T1>>(); }
        public EventMapInfoInterface<T1, T2>     TakeInterface<T1, T2>()     { return Take<EventMapInfoInterface<T1, T2>>(); }
        public EventMapInfoInterface<T1, T2, T3> TakeInterface<T1, T2, T3>() { return Take<EventMapInfoInterface<T1, T2, T3>>(); }

        public EventMapInfoExact             TakeExact()             { return Take<EventMapInfoExact>(); }
        public EventMapInfoExact<T1>         TakeExact<T1>()         { return Take<EventMapInfoExact<T1>>(); }
        public EventMapInfoExact<T1, T2>     TakeExact<T1, T2>()     { return Take<EventMapInfoExact<T1, T2>>(); }
        public EventMapInfoExact<T1, T2, T3> TakeExact<T1, T2, T3>() { return Take<EventMapInfoExact<T1, T2, T3>>(); }
    }
}