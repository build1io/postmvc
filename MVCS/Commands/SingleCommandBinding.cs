// @formatter:off

using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class SingleCommandBinding
    {
        public SingleCommandBinding OnComplete(Event @event) { throw new NotImplementedException(); }

        public SingleCommandBinding OnBreak(Event @event) { throw new NotImplementedException(); }

        public SingleCommandBinding OnFail(Event<Exception> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding OnFail(Event @event)            { throw new NotImplementedException(); }

        public void Execute() { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1>
    {
        public SingleCommandBinding<T1> OnComplete(Event<T1> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1> OnComplete(Event @event)     { throw new NotImplementedException(); }

        public SingleCommandBinding<T1> OnBreak(Event<T1> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1> OnBreak(Event @event)     { throw new NotImplementedException(); }

        public SingleCommandBinding<T1> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1> OnFail(Event @event)            { throw new NotImplementedException(); }

        public void Execute(T1 param01) { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1, T2>
    {
        public SingleCommandBinding<T1, T2> OnComplete(Event<T1, T2> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2> OnComplete(Event<T1> @event)     { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2> OnComplete(Event @event)         { throw new NotImplementedException(); }

        public SingleCommandBinding<T1, T2> OnBreak(Event<T1, T2> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2> OnBreak(Event<T1> @event)     { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2> OnBreak(Event @event)         { throw new NotImplementedException(); }

        public SingleCommandBinding<T1, T2> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2> OnFail(Event @event)            { throw new NotImplementedException(); }

        public void Execute(T1 param01, T2 param02) { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1, T2, T3>
    {
        public SingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)     { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnComplete(Event<T1> @event)         { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnComplete(Event @event)             { throw new NotImplementedException(); }

        public SingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)     { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnBreak(Event<T1> @event)         { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnBreak(Event @event)             { throw new NotImplementedException(); }

        public SingleCommandBinding<T1, T2, T3> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }
        public SingleCommandBinding<T1, T2, T3> OnFail(Event @event)            { throw new NotImplementedException(); }

        public void Execute(T1 param01, T2 param02, T3 param03) { throw new NotImplementedException(); }
    }
}

// @formatter:on