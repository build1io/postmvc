using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding To<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
        
        public CommandBinding To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
        
        public CommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
        
        public CommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2, T3> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
        
        public CommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public CommandBinding<T1, T2, T3> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }
}