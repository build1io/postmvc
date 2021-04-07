using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public interface IUnityEventBindingTo
    {
        IUnityEventBindingTo ToEvent(Event @event);
        IUnityEventBindingTo ToAction(Action action);
    }
}