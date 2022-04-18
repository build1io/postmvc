using Build1.PostMVC.Extensions.MVCS.Events;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public interface ICommandBinding : ICommandBindingBase
    {
        ICommandBinding And(Event @event);
        ICommandBinding Bind(Event @event);

        ICommandBinding To<TCommand>() where TCommand : Command, new();
        ICommandBinding To0<TCommand>() where TCommand : Command, new();

        ICommandBinding To1<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding To1<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding To1<TCommand>(bool param) where TCommand : Command<bool>, new();

        ICommandBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();
        
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        ICommandBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();
        
        ICommandBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        ICommandBinding OnComplete(Event @event);
        ICommandBinding OnBreak(Event @event);
    }

    public interface ICommandBinding<T1> : ICommandBindingBase
    {
        ICommandBinding<T1> And(Event<T1> @event);
        ICommandBinding<T1> Bind(Event<T1> @event);

        ICommandBinding<T1> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1> To<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding<T1> To<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding<T1> To<TCommand>(bool param) where TCommand : Command<bool>, new();
        ICommandBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        ICommandBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        ICommandBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        ICommandBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        ICommandBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        ICommandBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        ICommandBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        ICommandBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        ICommandBinding<T1> OnComplete(Event<T1> @event);
        ICommandBinding<T1> OnComplete(Event @event);

        ICommandBinding<T1> OnBreak(Event<T1> @event);
        ICommandBinding<T1> OnBreak(Event @event);
    }

    public interface ICommandBinding<T1, T2> : ICommandBindingBase
    {
        ICommandBinding<T1, T2> And(Event<T1, T2> @event);
        ICommandBinding<T1, T2> Bind(Event<T1, T2> @event);

        ICommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        ICommandBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new();
        ICommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new();

        ICommandBinding<T1, T2> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        ICommandBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        
        ICommandBinding<T1, T2> To<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        ICommandBinding<T1, T2> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        
        ICommandBinding<T1, T2> To<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        ICommandBinding<T1, T2> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        
        ICommandBinding<T1, T2> To<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();
        ICommandBinding<T1, T2> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();

        ICommandBinding<T1, T2> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();
        ICommandBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        ICommandBinding<T1, T2> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        ICommandBinding<T1, T2> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        ICommandBinding<T1, T2> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        ICommandBinding<T1, T2> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        
        ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

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

        ICommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new();

        ICommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        ICommandBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand>(int param) where TCommand : Command<T1, int>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand>(float param) where TCommand : Command<T1, float>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand>(bool param) where TCommand : Command<T1, bool>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        ICommandBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        ICommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        ICommandBinding<T1, T2, T3> To<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        ICommandBinding<T1, T2, T3> To<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        ICommandBinding<T1, T2, T3> To<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        ICommandBinding<T1, T2, T3> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        ICommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        ICommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();
        
        ICommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        ICommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

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