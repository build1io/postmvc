using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands.Impl
{
    public abstract class CommandParamsBase
    {
        internal abstract void DispatchParams(IEventDispatcher dispatcher, EventBase @event);
    }
}