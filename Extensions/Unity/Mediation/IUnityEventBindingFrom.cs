using System;
using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.Unity.Mediation
{
    public interface IUnityEventBindingFrom
    {
        IUnityEventBindingFrom FromEvent(Event @event);
        IUnityEventBindingFrom FromAction(Action action);
    }
}