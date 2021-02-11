using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding To<TCommand>() where TCommand : class, ICommand, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
    }
    
    public sealed class CommandBinding<T1> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1> To<TCommand>() where TCommand : class, ICommand<T1>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
    }
    
    public sealed class CommandBinding<T1, T2> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2> To<TCommand>() where TCommand : class, ICommand<T1, T2>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
    }
    
    public sealed class CommandBinding<T1, T2, T3> : CommandBindingBase
    {
        public CommandBinding(EventBase type) : base(type)
        {
        }

        public CommandBinding<T1, T2, T3> To<TCommand>() where TCommand : class, ICommand<T1, T2, T3>, new()
        {
            Commands.Add(typeof(TCommand));
            return this;
        }
    }
}