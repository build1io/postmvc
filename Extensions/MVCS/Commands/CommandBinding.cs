using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase, ICommandBinding
    {
        public CommandBinding(EventBase type, ICommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding And(Event @event)  { return new CommandBindingComposite(this).And(@event); }
        public ICommandBinding Bind(Event @event) { return new CommandBindingComposite(this).Bind(@event); }

        public ICommandBinding To<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1> : CommandBindingBase, ICommandBinding<T1>
    {
        public CommandBinding(EventBase type, ICommandBinder binder) : base(type, binder)
        {
        }
        
        public ICommandBinding<T1> And(Event<T1> @event)  { return new CommandBindingComposite<T1>(this).And(@event); }
        public ICommandBinding<T1> Bind(Event<T1> @event) { return new CommandBindingComposite<T1>(this).Bind(@event); }

        public ICommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2> : CommandBindingBase, ICommandBinding<T1, T2>
    {
        public CommandBinding(EventBase type, ICommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding<T1, T2> And(Event<T1, T2> @event)  { return new CommandBindingComposite<T1, T2>(this).And(@event); }
        public ICommandBinding<T1, T2> Bind(Event<T1, T2> @event) { return new CommandBindingComposite<T1, T2>(this).Bind(@event); }

        public ICommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2, T3> : CommandBindingBase, ICommandBinding<T1, T2, T3>
    {
        public CommandBinding(EventBase type, ICommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding<T1, T2, T3> And(Event<T1, T2, T3> @event)  { return new CommandBindingComposite<T1, T2, T3>(this).And(@event); }
        public ICommandBinding<T1, T2, T3> Bind(Event<T1, T2, T3> @event) { return new CommandBindingComposite<T1, T2, T3>(this).Bind(@event); }

        public ICommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public ICommandBinding<T1, T2, T3> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }
    }
}