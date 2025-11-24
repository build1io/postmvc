using System;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class EventHandleCommand : IEventHandleFull
    {
        public bool IsRetained => _command.IsRetained;

        private readonly CommandBase _command;
        private          int         _retainCounter;

        public EventHandleCommand(CommandBase command)
        {
            _command = command;
        }

        public void Retain()
        {
            if (!IsRetained)
                _command.RetainInternal();
            
            _retainCounter++;
        }

        public void Release()
        {
            _retainCounter--;
            
            if (_retainCounter == 0 && !_command.IsBreak)
                _command.ReleaseInternal();
        }

        public void Break()
        {
            _command.BreakInternal();
        }

        public void Fail(Exception exception)
        {
            _command.FailInternal(exception);
        }
    }
}