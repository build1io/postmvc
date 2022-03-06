using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public interface ICommandBinding : ICommandBindingBase
    {
        ICommandBinding And(Event @event);
        ICommandBinding Bind(Event @event);

        ICommandBinding To<TCommand>() where TCommand : Command, new();
        ICommandBinding To0<TCommand>() where TCommand : Command, new();

        ICommandBinding OnComplete(Event @event);

        ICommandBinding OnBreak(Event @event);
    }

    public interface ICommandBinding<T1> : ICommandBindingBase
    {
        ICommandBinding<T1> And(Event<T1> @event);
        ICommandBinding<T1> Bind(Event<T1> @event);

        ICommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1> OnComplete(Event<T1> @event);
        ICommandBinding<T1> OnComplete(Event @event);

        ICommandBinding<T1> OnBreak(Event<T1> @event);
        ICommandBinding<T1> OnBreak(Event @event);
    }

    public interface ICommandBinding<T1, T2> : ICommandBindingBase
    {
        ICommandBinding<T1, T2> And(Event<T1, T2> @event);
        ICommandBinding<T1, T2> Bind(Event<T1, T2> @event);

        ICommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new();
        ICommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new();
        ICommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1, T2> OnComplete(Event<T1, T2> @event);
        ICommandBinding<T1, T2> OnComplete(Event<T1> @event);
        ICommandBinding<T1, T2> OnComplete(Event @event);

        ICommandBinding<T1, T2> OnBreak(Event<T1, T2> @event);
        ICommandBinding<T1, T2> OnBreak(Event<T1> @event);
        ICommandBinding<T1, T2> OnBreak(Event @event);
    }

    public interface ICommandBinding<T1, T2, T3> : ICommandBindingBase
    {
        ICommandBinding<T1, T2, T3> And(Event<T1, T2, T3> @event);
        ICommandBinding<T1, T2, T3> Bind(Event<T1, T2, T3> @event);

        ICommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new();
        ICommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event<T1> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event @event);

        ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event<T1> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event @event);
    }
}