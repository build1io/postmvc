using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public abstract class CommandParamsBase
    {
        public abstract bool TryExecuteCommand(CommandBase commandBase);
        public abstract void DispatchParams(IEventDispatcher dispatcher, EventBase @event);
    }
}