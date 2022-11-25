using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public interface ISingleCommandBinding
    {
        ISingleCommandBinding OnComplete(Event @event);
        
        ISingleCommandBinding OnBreak(Event @event);
        
        ISingleCommandBinding OnFail(Event<Exception> @event);
        ISingleCommandBinding OnFail(Event @event);
        
        void Execute();
    }
    
    public interface ISingleCommandBinding<T1>
    {
        ISingleCommandBinding<T1> OnComplete(Event<T1> @event);
        ISingleCommandBinding<T1> OnComplete(Event @event);

        ISingleCommandBinding<T1> OnBreak(Event<T1> @event);
        ISingleCommandBinding<T1> OnBreak(Event @event);
        
        ISingleCommandBinding<T1> OnFail(Event<Exception> @event);
        ISingleCommandBinding<T1> OnFail(Event @event);
        
        void Execute(T1 param01);
    }
    
    public interface ISingleCommandBinding<T1, T2>
    {
        ISingleCommandBinding<T1, T2> OnComplete(Event<T1, T2> @event);
        ISingleCommandBinding<T1, T2> OnComplete(Event<T1> @event);
        ISingleCommandBinding<T1, T2> OnComplete(Event @event);

        ISingleCommandBinding<T1, T2> OnBreak(Event<T1, T2> @event);
        ISingleCommandBinding<T1, T2> OnBreak(Event<T1> @event);
        ISingleCommandBinding<T1, T2> OnBreak(Event @event);
        
        ISingleCommandBinding<T1, T2> OnFail(Event<Exception> @event);
        ISingleCommandBinding<T1, T2> OnFail(Event @event);
        
        void Execute(T1 param01, T2 param02);
    }
    
    public interface ISingleCommandBinding<T1, T2, T3>
    {
        ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event);
        ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event);
        ISingleCommandBinding<T1, T2, T3> OnComplete(Event<T1> @event);
        ISingleCommandBinding<T1, T2, T3> OnComplete(Event @event);

        ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event);
        ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event);
        ISingleCommandBinding<T1, T2, T3> OnBreak(Event<T1> @event);
        ISingleCommandBinding<T1, T2, T3> OnBreak(Event @event);
        
        ISingleCommandBinding<T1, T2, T3> OnFail(Event<Exception> @event);
        ISingleCommandBinding<T1, T2, T3> OnFail(Event @event);
        
        void Execute(T1 param01, T2 param02, T3 param03);
    }
}