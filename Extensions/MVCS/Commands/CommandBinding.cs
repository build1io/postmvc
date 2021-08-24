using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase
    {
        internal Event CompleteEvent { get; private set; }

        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding To<TCommand>() where TCommand : class, ICommand, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1> : CommandBindingBase
    {
        internal Event<T1> CompleteEvent { get; private set; }

        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1> To<TCommand>() where TCommand : class, ICommand<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2> : CommandBindingBase
    {
        internal Event<T1, T2> CompleteEvent { get; private set; }

        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2> To<TCommand>() where TCommand : class, ICommand<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }
    }

    public sealed class CommandBinding<T1, T2, T3> : CommandBindingBase
    {
        internal Event<T1, T2, T3> CompleteEvent { get; private set; }

        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2, T3> To<TCommand>() where TCommand : class, ICommand<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }

        public CommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event)
        {
            CompleteEvent = @event;
            return this;
        }
    }
}