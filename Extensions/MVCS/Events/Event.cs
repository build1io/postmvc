using Build1.PostMVC.Core.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.Extensions.MVCS.Events
{
    public sealed class Event : EventBase { }
    public sealed class Event<T1>: EventBase { }
    public sealed class Event<T1, T2>: EventBase { }
    public sealed class Event<T1, T2, T3>: EventBase { }
}