using System;
using Build1.PostMVC.Core.MVCS.Commands.Impl;

namespace Build1.PostMVC.Core.MVCS.Events.Impl
{
    public sealed class EventHandleCommand : IEventHandleFull
    {
        public bool IsRetained => _command.IsRetained;

        private readonly CommandBase _command;

        public EventHandleCommand(CommandBase command)
        {
            _command = command;
        }

        public void Retain()                  { _command.RetainInternal(); }
        public void Release()                 { _command.ReleaseInternal(); }
        public void Break()                   { _command.BreakInternal(); }
        public void Fail(Exception exception) { _command.FailInternal(exception); }
    }
}