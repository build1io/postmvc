using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class SingleCommandBinding : ISingleCommandBinding
    {
        public ISingleCommandBinding OnComplete(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding OnBreak(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding OnFail(Event<Exception> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding OnFail(Event @event) { throw new NotImplementedException(); }

        public void Execute() { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1> : ISingleCommandBinding<T1>
    {
        public ISingleCommandBinding<T1> OnComplete(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1> OnComplete(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1> OnBreak(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1> OnBreak(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1> OnFail(Event @event) { throw new NotImplementedException(); }

        public void Execute(T1 param01) { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1, T2> : ISingleCommandBinding<T1, T2>
    {
        public ISingleCommandBinding<T1, T2> OnComplete(Event<T1, T2> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnComplete(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnComplete(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnBreak(Event<T1, T2> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnBreak(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnBreak(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2> OnFail(Event @event) { throw new NotImplementedException(); }

        public void Execute(T1 param01, T2 param02) { throw new NotImplementedException(); }
    }
    
    public sealed class SingleCommandBinding<T1, T2, T3> : ISingleCommandBinding<T1, T2, T3>
    {
        public ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnComplete(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnBreak(Event @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnFail(Event<Exception> @event) { throw new NotImplementedException(); }

        public ISingleCommandBinding<T1, T2, T3> OnFail(Event @event) { throw new NotImplementedException(); }

        public void Execute(T1 param01, T2 param02, T3 param03) { throw new NotImplementedException(); }
    }
}