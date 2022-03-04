using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class CommandBindingBase
    {
        internal EventBase       Event            { get; }
        internal List<Type>      Commands         { get; }
        internal int             CommandsExecuted { get; private set; }
        internal int             CommandsReleased { get; private set; }
        internal List<Exception> CommandsFailed   { get; }
        internal EventBase       CompleteEvent    { get; set; }
        internal EventBase       BreakEvent       { get; set; }
        internal EventBase       FailEvent        { get; private set; }
        internal bool            IsSequence       { get; private set; }
        internal bool            IsOnce           { get; private set; }
        internal bool            IsUnbindOnQuit   { get; private set; }

        internal bool IsExecuting { get; private set; }
        internal bool IsBreak     { get; private set; }

        internal bool HasFails => CommandsFailed.Count > 0;

        protected CommandBindingBase(EventBase type)
        {
            Event = type;
            Commands = new List<Type>();
            CommandsFailed = new List<Exception>();
        }

        public void StartExecution()
        {
            IsExecuting = true;
        }

        public void FinishExecution()
        {
            CommandsExecuted = 0;
            CommandsReleased = 0;
            CommandsFailed.Clear();
            IsExecuting = false;
            IsBreak = false;
        }

        public void RegisterCommandExecute()
        {
            CommandsExecuted++;
        }

        public void RegisterCommandBreak()
        {
            IsBreak = true;
        }

        public void RegisterCommandRelease()
        {
            CommandsReleased++;
        }

        public void RegisterCommandException(Exception exception)
        {
            CommandsFailed.Add(exception);
        }

        public bool CheckAllExecuted()
        {
            return Commands.Count == CommandsExecuted;
        }

        public bool CheckAllReleased()
        {
            if (IsSequence)
                return Commands.Count == CommandsReleased || HasFails;
            return Commands.Count == CommandsReleased + CommandsFailed.Count;
        }

        /*
         * On Fail.
         */

        public CommandBindingBase OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }
        
        public CommandBindingBase OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }
        
        /*
         * Configuration.
         */

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