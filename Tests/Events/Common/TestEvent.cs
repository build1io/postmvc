using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.Tests.Events.Common
{
    public abstract class TestEvent
    {
        public static readonly Event                    Event00   = new(typeof(TestEvent), nameof(Event00));
        public static readonly Event<int>               Event01   = new(typeof(TestEvent), nameof(Event01));
        public static readonly Event<int, string>       Event02   = new(typeof(TestEvent), nameof(Event02));
        public static readonly Event<int, string, bool> Event03   = new(typeof(TestEvent), nameof(Event03));
        public static readonly Event<Exception>         EventFail = new(typeof(TestEvent), nameof(Event00));
    }
}