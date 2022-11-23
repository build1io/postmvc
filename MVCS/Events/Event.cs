using System;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Events
{
    public sealed class Event : EventBase
    {
        public Event() { }
        public Event(Type type) : base(type.FullName) { }
        public Event(string name) : base(name) { }
        public Event(Type type, string name) : base(type, name) { }
    }

    public sealed class Event<T1> : EventBase
    {
        public Event() { }
        public Event(Type type) : base(type.FullName) { }
        public Event(string name) : base(name) { }
        public Event(Type type, string name) : base(type, name) { }
    }

    public sealed class Event<T1, T2> : EventBase
    {
        public Event() { }
        public Event(Type type) : base(type.FullName) { }
        public Event(string name) : base(name) { }
        public Event(Type type, string name) : base(type, name) { }
    }

    public sealed class Event<T1, T2, T3> : EventBase
    {
        public Event() { }
        public Event(Type type) : base(type.FullName) { }
        public Event(string name) : base(name) { }
        public Event(Type type, string name) : base(type, name) { }
    }
}