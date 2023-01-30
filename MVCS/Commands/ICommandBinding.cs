using System;
using Build1.PostMVC.Core.MVCS.Events;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public interface ICommandBinding : ICommandBindingBase
    {
        ICommandBinding And(Event @event);
        ICommandBinding Bind(Event @event);

        ICommandBinding To<TCommand>() where TCommand : Command, new();
        ICommandBinding To0<TCommand>() where TCommand : Command, new();

        ICommandBinding To1<TCommand>(int param01) where TCommand : Command<int>, new();
        ICommandBinding To1<TCommand>(float param01) where TCommand : Command<float>, new();
        ICommandBinding To1<TCommand>(bool param01) where TCommand : Command<bool>, new();
        ICommandBinding To1<TCommand>(string param01) where TCommand : Command<string>, new();
        ICommandBinding To1<TCommand>(Event param01) where TCommand : Command<Event>, new();
        ICommandBinding To1<TCommand>(Exception param01) where TCommand : Command<Exception>, new();
        
        ICommandBinding To1<TCommand>(Func<int> param01) where TCommand : Command<int>, new();
        ICommandBinding To1<TCommand>(Func<float> param01) where TCommand : Command<float>, new();
        ICommandBinding To1<TCommand>(Func<bool> param01) where TCommand : Command<bool>, new();
        ICommandBinding To1<TCommand>(Func<string> param01) where TCommand : Command<string>, new();

        ICommandBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();
        ICommandBinding To1<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new();

        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        ICommandBinding To2<TCommand, TCP1>(TCP1 param01, string param02) where TCommand : Command<TCP1, string>, new();
        ICommandBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        ICommandBinding To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02, string param03) where TCommand : Command<TCP1, TCP2, string>, new();
        ICommandBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        ICommandBinding TriggerCondition(Func<bool> predicate);

        ICommandBinding OnComplete(Event @event);
        ICommandBinding OnBreak(Event @event);
        ICommandBinding OnFail(Event<Exception> @event);
        ICommandBinding OnFail(Event @event);

        ICommandBinding InParallel();
        ICommandBinding InSequence();

        ICommandBinding Once();
        ICommandBinding Once(OnceBehavior behavior);

        ICommandBinding UnbindOnQuit();
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
        ICommandBinding<T1> To<TCommand>(string param) where TCommand : Command<string>, new();
        ICommandBinding<T1> To<TCommand>(Event param) where TCommand : Command<Event>, new();
        ICommandBinding<T1> To<TCommand>(Exception param) where TCommand : Command<Exception>, new();
        
        ICommandBinding<T1> To<TCommand>(Func<int> param) where TCommand : Command<int>, new();
        ICommandBinding<T1> To<TCommand>(Func<float> param) where TCommand : Command<float>, new();
        ICommandBinding<T1> To<TCommand>(Func<bool> param) where TCommand : Command<bool>, new();
        ICommandBinding<T1> To<TCommand>(Func<string> param) where TCommand : Command<string>, new();
        
        ICommandBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();
        ICommandBinding<T1> To<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new();
        ICommandBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new();
        ICommandBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new();
        ICommandBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new();
        ICommandBinding<T1> To1<TCommand>(string param) where TCommand : Command<string>, new();
        ICommandBinding<T1> To1<TCommand>(Event param) where TCommand : Command<Event>, new();
        ICommandBinding<T1> To1<TCommand>(Exception param) where TCommand : Command<Exception>, new();
        
        ICommandBinding<T1> To1<TCommand>(Func<int> param) where TCommand : Command<int>, new();
        ICommandBinding<T1> To1<TCommand>(Func<float> param) where TCommand : Command<float>, new();
        ICommandBinding<T1> To1<TCommand>(Func<bool> param) where TCommand : Command<bool>, new();
        ICommandBinding<T1> To1<TCommand>(Func<string> param) where TCommand : Command<string>, new();
        
        ICommandBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new();
        ICommandBinding<T1> To1<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new();

        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new();
        ICommandBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new();
        ICommandBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new();
        ICommandBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new();
        ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new();
        ICommandBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new();

        ICommandBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        ICommandBinding<T1> To3<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<T1, TCP1, bool>, new();
        ICommandBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        ICommandBinding<T1> TriggerCondition(T1 value01);
        ICommandBinding<T1> TriggerCondition(Func<T1, bool> predicate);

        ICommandBinding<T1> OnComplete(Event<T1> @event);
        ICommandBinding<T1> OnComplete(Event @event);

        ICommandBinding<T1> OnBreak(Event<T1> @event);
        ICommandBinding<T1> OnBreak(Event @event);

        ICommandBinding<T1> OnFail(Event<Exception> @event);
        ICommandBinding<T1> OnFail(Event @event);

        ICommandBinding<T1> InParallel();
        ICommandBinding<T1> InSequence();

        ICommandBinding<T1> Once();
        ICommandBinding<T1> Once(OnceBehavior behavior);

        ICommandBinding<T1> UnbindOnQuit();
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
        ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new();
        ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new();

        ICommandBinding<T1, T2> TriggerCondition(T1 value01, T2 value02);
        ICommandBinding<T1, T2> TriggerCondition(Func<T1, T2, bool> predicate);

        ICommandBinding<T1, T2> OnComplete(Event<T1, T2> @event);
        ICommandBinding<T1, T2> OnComplete(Event<T1> @event);
        ICommandBinding<T1, T2> OnComplete(Event @event);

        ICommandBinding<T1, T2> OnBreak(Event<T1, T2> @event);
        ICommandBinding<T1, T2> OnBreak(Event<T1> @event);
        ICommandBinding<T1, T2> OnBreak(Event @event);

        ICommandBinding<T1, T2> OnFail(Event<Exception> @event);
        ICommandBinding<T1, T2> OnFail(Event @event);

        ICommandBinding<T1, T2> InParallel();
        ICommandBinding<T1, T2> InSequence();

        ICommandBinding<T1, T2> Once();
        ICommandBinding<T1, T2> Once(OnceBehavior behavior);

        ICommandBinding<T1, T2> UnbindOnQuit();
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

        ICommandBinding<T1, T2, T3> TriggerCondition(T1 value01, T2 value02, T3 value03);
        ICommandBinding<T1, T2, T3> TriggerCondition(Func<T1, T2, T3, bool> predicate);

        ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event<T1> @event);
        ICommandBinding<T1, T2, T3> OnComplete(Event @event);

        ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event<T1> @event);
        ICommandBinding<T1, T2, T3> OnBreak(Event @event);

        ICommandBinding<T1, T2, T3> OnFail(Event<Exception> @event);
        ICommandBinding<T1, T2, T3> OnFail(Event @event);

        ICommandBinding<T1, T2, T3> InParallel();
        ICommandBinding<T1, T2, T3> InSequence();

        ICommandBinding<T1, T2, T3> Once();
        ICommandBinding<T1, T2, T3> Once(OnceBehavior behavior);

        ICommandBinding<T1, T2, T3> UnbindOnQuit();
    }
}