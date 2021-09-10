using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class CommandBindingBase
    {
        internal EventBase        Event            { get; }
        internal List<Type>       Commands         { get; }
        internal int              CommandsReleased { get; private set; }
        internal List<Exception>  CommandsFailed   { get; private set; }
        internal Event<Exception> FailEvent        { get; private set; }
        internal bool             IsSequence       { get; private set; }
        internal bool             IsOnce           { get; private set; }
        internal bool             IsUnbindOnQuit   { get; private set; }

        internal bool IsExecuting { get; private set; }
        internal bool HasFails    => CommandsFailed != null;

        protected CommandBindingBase(EventBase type)
        {
            Event = type;
            Commands = new List<Type>();
        }

        public void StartExecution()
        {
            IsExecuting = true;
        }

        public void FinishExecution()
        {
            CommandsReleased = 0;
            CommandsFailed = null;
            IsExecuting = false;
        }

        public void RegisterCommandRelease()
        {
            CommandsReleased++;
        }

        public void RegisterCommandException(Exception exception)
        {
            CommandsFailed ??= new List<Exception>();
            CommandsFailed.Add(exception);
        }

        public bool CheckAllReleased()
        {
            if (IsSequence)
                return Commands.Count == CommandsReleased || CommandsFailed != null;
            return Commands.Count == CommandsReleased + (CommandsFailed?.Count ?? 0);
        }

        /*
         * Configuration.
         */

        public CommandBindingBase OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public CommandBindingBase InParallel()
        {
            IsSequence = false;
            return this;
        }

        public CommandBindingBase InSequence()
        {
            IsSequence = true;
            return this;
        }

        public CommandBindingBase Once()
        {
            IsOnce = true;
            return this;
        }

        public CommandBindingBase UnbindOnQuit()
        {
            IsUnbindOnQuit = true;
            return this;
        }
    }
}