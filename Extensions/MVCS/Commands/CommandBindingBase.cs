using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class CommandBindingBase : ICommandBindingBase
    {
        internal EventBase       Event            { get; }
        internal ICommandBinder  CommandBinder    { get; }
        internal List<Type>      Commands         { get; }
        internal int             CommandsExecuted { get; private set; }
        internal int             CommandsReleased { get; private set; }
        internal List<Exception> CommandsFailed   { get; }
        internal EventBase       CompleteEvent    { get; set; }
        internal EventBase       BreakEvent       { get; set; }
        internal EventBase       FailEvent        { get; private set; }
        internal bool            IsSequence       { get; private set; }
        internal OnceBehavior    OnceBehavior     { get; private set; }
        internal bool            IsUnbindOnQuit   { get; private set; }

        internal bool IsExecuting { get; private set; }
        internal bool IsBreak     { get; private set; }

        internal bool HasFails => CommandsFailed.Count > 0;

        protected CommandBindingBase(EventBase type, ICommandBinder binder)
        {
            Event = type;
            CommandBinder = binder;
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

        public ICommandBindingBase OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public ICommandBindingBase OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }

        /*
         * Configuration.
         */

        public ICommandBindingBase InParallel()
        {
            IsSequence = false;
            return this;
        }

        public ICommandBindingBase InSequence()
        {
            IsSequence = true;
            return this;
        }

        public ICommandBindingBase Once()
        {
            OnceBehavior = OnceBehavior.Default;
            return this;
        }
        
        public ICommandBindingBase Once(OnceBehavior behavior)
        {
            OnceBehavior = behavior;
            return this;
        }

        public ICommandBindingBase UnbindOnQuit()
        {
            IsUnbindOnQuit = true;
            return this;
        }
    }
}