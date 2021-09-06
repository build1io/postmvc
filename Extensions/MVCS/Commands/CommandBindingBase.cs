using System;
using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class CommandBindingBase
    {
        internal EventBase        Event          { get; }
        internal List<Type>       Commands       { get; }
        internal Event<Exception> FailEvent      { get; private set; }
        internal bool             IsSequence     { get; private set; }
        internal bool             IsOnce         { get; private set; }
        internal bool             IsUnbindOnQuit { get; private set; }

        protected CommandBindingBase(EventBase type)
        {
            Event = type;
            Commands = new List<Type>();
        }

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