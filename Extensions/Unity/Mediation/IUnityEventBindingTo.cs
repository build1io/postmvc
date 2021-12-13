using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public interface IUnityEventBindingTo
    {
        IUnityEventBindingTo ToEvent(Event @event);
        IUnityEventBindingTo ToAction(Action action);
    }

    public interface IUnityEventBindingTo<T1>
    {
        IUnityEventBindingTo<T1> ToEvent(Event<T1> @event);
        IUnityEventBindingTo<T1> ToAction(Action<T1> action);
    }
    
    public interface IUnityEventBindingTo<T1, T2>
    {
        IUnityEventBindingTo<T1, T2> ToEvent(Event<T1, T2> @event);
        IUnityEventBindingTo<T1, T2> ToAction(Action<T1, T2> action);
    }
    
    public interface IUnityEventBindingTo<T1, T2, T3>
    {
        IUnityEventBindingTo<T1, T2, T3> ToEvent(Event<T1, T2, T3> @event);
        IUnityEventBindingTo<T1, T2, T3> ToAction(Action<T1, T2, T3> action);
    }
}