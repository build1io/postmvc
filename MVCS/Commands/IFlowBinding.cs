using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public interface IFlowBinding
    {
        IFlowBinding To<TCommand>() where TCommand : Command, new();
        IFlowBinding To0<TCommand>() where TCommand : Command, new();

        IFlowBinding To1<TCommand>(int param01) where TCommand : Command<int>, new();
        IFlowBinding To1<TCommand>(float param01) where TCommand : Command<float>, new();
        IFlowBinding To1<TCommand>(bool param01) where TCommand : Command<bool>, new();
        IFlowBinding To1<TCommand>(string param01) where TCommand : Command<string>, new();
        IFlowBinding To1<TCommand>(Exception param01) where TCommand : Command<Exception>, new();
        IFlowBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        IFlowBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        IFlowBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        IFlowBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        IFlowBinding To2<TCommand, TCP1>(TCP1 param01, string param02) where TCommand : Command<TCP1, string>, new();
        IFlowBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        IFlowBinding To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02, string param03) where TCommand : Command<TCP1, TCP2, string>, new();
        IFlowBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        IFlowBinding OnComplete(Event @event);
        
        IFlowBinding OnBreak(Event @event);
        
        IFlowBinding OnFail(Event<Exception> @event);
        IFlowBinding OnFail(Event @event);

        IFlowBinding InParallel();
        IFlowBinding InSequence();

        void Execute();
    }
    
    public interface IFlowBinding<T1>
    {
        IFlowBinding<T1> To0<TCommand>() where TCommand : Command, new();

        IFlowBinding<T1> To<TCommand>() where TCommand : Command<T1>, new();
        IFlowBinding<T1> To<TCommand>(int param) where TCommand : Command<int>, new();
        IFlowBinding<T1> To<TCommand>(float param) where TCommand : Command<float>, new();
        IFlowBinding<T1> To<TCommand>(bool param) where TCommand : Command<bool>, new();
        IFlowBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        IFlowBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new();
        IFlowBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new();
        IFlowBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new();
        IFlowBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        IFlowBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        IFlowBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        IFlowBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        IFlowBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();
        IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        IFlowBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        IFlowBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        IFlowBinding<T1> To3<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<T1, TCP1, bool>, new();
        IFlowBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        IFlowBinding<T1> OnComplete(Event<T1> @event);
        IFlowBinding<T1> OnComplete(Event @event);

        IFlowBinding<T1> OnBreak(Event<T1> @event);
        IFlowBinding<T1> OnBreak(Event @event);
        
        IFlowBinding<T1> OnFail(Event<Exception> @event);
        IFlowBinding<T1> OnFail(Event @event);

        IFlowBinding<T1> InParallel();
        IFlowBinding<T1> InSequence();

        void Execute(T1 param01);
    }
    
    public interface IFlowBinding<T1, T2>
    {
        IFlowBinding<T1, T2> To0<TCommand>() where TCommand : Command, new();

        IFlowBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new();
        IFlowBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new();
        IFlowBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new();
        IFlowBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        IFlowBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        IFlowBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new();
        IFlowBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new();

        IFlowBinding<T1, T2> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        IFlowBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();

        IFlowBinding<T1, T2> To<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        IFlowBinding<T1, T2> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new();

        IFlowBinding<T1, T2> To<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        IFlowBinding<T1, T2> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new();

        IFlowBinding<T1, T2> To<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();
        IFlowBinding<T1, T2> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();

        IFlowBinding<T1, T2> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();
        IFlowBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        IFlowBinding<T1, T2> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        IFlowBinding<T1, T2> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        IFlowBinding<T1, T2> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        IFlowBinding<T1, T2> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        IFlowBinding<T1, T2> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        IFlowBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        IFlowBinding<T1, T2> OnComplete(Event<T1, T2> @event);
        IFlowBinding<T1, T2> OnComplete(Event<T1> @event);
        IFlowBinding<T1, T2> OnComplete(Event @event);

        IFlowBinding<T1, T2> OnBreak(Event<T1, T2> @event);
        IFlowBinding<T1, T2> OnBreak(Event<T1> @event);
        IFlowBinding<T1, T2> OnBreak(Event @event);
        
        IFlowBinding<T1, T2> OnFail(Event<Exception> @event);
        IFlowBinding<T1, T2> OnFail(Event @event);

        IFlowBinding<T1, T2> InParallel();
        IFlowBinding<T1, T2> InSequence();

        void Execute(T1 param01, T2 param02);
    }
    
    public interface IFlowBinding<T1, T2, T3>
    {
        IFlowBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new();

        IFlowBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new();
        IFlowBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new();
        IFlowBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new();
        IFlowBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        IFlowBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();

        IFlowBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new();
        IFlowBinding<T1, T2, T3> To2<TCommand>(int param) where TCommand : Command<T1, int>, new();
        IFlowBinding<T1, T2, T3> To2<TCommand>(float param) where TCommand : Command<T1, float>, new();
        IFlowBinding<T1, T2, T3> To2<TCommand>(bool param) where TCommand : Command<T1, bool>, new();
        IFlowBinding<T1, T2, T3> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        IFlowBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        IFlowBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        IFlowBinding<T1, T2, T3> To<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        IFlowBinding<T1, T2, T3> To<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        IFlowBinding<T1, T2, T3> To<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        IFlowBinding<T1, T2, T3> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        IFlowBinding<T1, T2, T3> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        IFlowBinding<T1, T2, T3> To<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        IFlowBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        IFlowBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        IFlowBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event);
        IFlowBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event);
        IFlowBinding<T1, T2, T3> OnComplete(Event<T1> @event);
        IFlowBinding<T1, T2, T3> OnComplete(Event @event);

        IFlowBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event);
        IFlowBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event);
        IFlowBinding<T1, T2, T3> OnBreak(Event<T1> @event);
        IFlowBinding<T1, T2, T3> OnBreak(Event @event);
        
        IFlowBinding<T1, T2, T3> OnFail(Event<Exception> @event);
        IFlowBinding<T1, T2, T3> OnFail(Event @event);

        IFlowBinding<T1, T2, T3> InParallel();
        IFlowBinding<T1, T2, T3> InSequence();

        void Execute(T1 param01, T2 param02, T3 param03);
    }
}