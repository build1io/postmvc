using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public interface IUnityEventBindingFrom
    {
        IUnityEventBindingFrom FromEvent(Event @event);
        IUnityEventBindingFrom FromAction(Action action);
    }

    public interface IUnityEventBindingFrom<T1>
    {
        IUnityEventBindingFrom<T1> FromEvent(Event<T1> @event);
        IUnityEventBindingFrom<T1> FromAction(Action<T1> action);
    }
    
    public interface IUnityEventBindingFrom<T1, T2>
    {
        IUnityEventBindingFrom<T1, T2> FromEvent(Event<T1, T2> @event);
        IUnityEventBindingFrom<T1, T2> FromAction(Action<T1, T2> action);
    }
    
    public interface IUnityEventBindingFrom<T1, T2, T3>
    {
        IUnityEventBindingFrom<T1, T2, T3> FromEvent(Event<T1, T2, T3> @event);
        IUnityEventBindingFrom<T1, T2, T3> FromAction(Action<T1, T2, T3> action);
    }
}