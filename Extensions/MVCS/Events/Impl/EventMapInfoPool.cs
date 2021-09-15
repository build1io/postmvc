using Build1.PostMVC.Utils.Pooling;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapInfoPool : Pool<IEventMapInfo>
    {
        public     EventMapInfo             Take()             { return base.Take<EventMapInfo>(); }
        public new EventMapInfo<T1>         Take<T1>()         { return base.Take<EventMapInfo<T1>>(); }
        public     EventMapInfo<T1, T2>     Take<T1, T2>()     { return base.Take<EventMapInfo<T1, T2>>(); }
        public     EventMapInfo<T1, T2, T3> Take<T1, T2, T3>() { return base.Take<EventMapInfo<T1, T2, T3>>(); }
    }
}