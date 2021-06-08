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
}