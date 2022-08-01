using System;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase, ICommandBinding
    {
        internal Func<bool> TriggerPredicate { get; private set; }

        public CommandBinding(EventBase type, CommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding And(Event @event)  { return new CommandBindingComposite(this).And(@event); }
        public ICommandBinding Bind(Event @event) { return new CommandBindingComposite(this).Bind(@event); }

        /*
         * Params 0.
         */

        public ICommandBinding To<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * 1 params.
         */

        public ICommandBinding To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * 2 param.
         */

        public ICommandBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public ICommandBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public ICommandBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public ICommandBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * 3 param.
         */

        public ICommandBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public ICommandBinding TriggerCondition(Func<bool> predicate)
        {
            TriggerPredicate = predicate;
            return this;
        }

        /*
         * Events.
         */

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
        internal T1             TriggerValue01   { get; private set; }
        internal bool           TriggerValuesSet { get; private set; }
        internal Func<T1, bool> TriggerPredicate { get; private set; }

        public CommandBinding(EventBase type, CommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding<T1> And(Event<T1> @event)  { return new CommandBindingComposite<T1>(this).And(@event); }
        public ICommandBinding<T1> Bind(Event<T1> @event) { return new CommandBindingComposite<T1>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public ICommandBinding<T1> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public ICommandBinding<T1> To<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1> To<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding<T1> To<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding<T1> To<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public ICommandBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public ICommandBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1> To3<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<T1, TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public ICommandBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public ICommandBinding<T1> TriggerValue(T1 value01)
        {
            TriggerValue01 = value01;
            TriggerValuesSet = true;
            return this;
        }

        public ICommandBinding<T1> TriggerCondition(Func<T1, bool> predicate)
        {
            TriggerPredicate = predicate;
            return this;
        }

        /*
         * Events.
         */

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
        internal T1                 TriggerValue01   { get; private set; }
        internal T2                 TriggerValue02   { get; private set; }
        internal bool               TriggerValuesSet { get; private set; }
        internal Func<T1, T2, bool> TriggerPredicate { get; private set; }

        public CommandBinding(EventBase type, CommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding<T1, T2> And(Event<T1, T2> @event)  { return new CommandBindingComposite<T1, T2>(this).And(@event); }
        public ICommandBinding<T1, T2> Bind(Event<T1, T2> @event) { return new CommandBindingComposite<T1, T2>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public ICommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public ICommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public ICommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public ICommandBinding<T1, T2> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public ICommandBinding<T1, T2> TriggerValues(T1 value01, T2 value02)
        {
            TriggerValue01 = value01;
            TriggerValue02 = value02;
            TriggerValuesSet = true;
            return this;
        }

        public ICommandBinding<T1, T2> TriggerCondition(Func<T1, T2, bool> predicate)
        {
            TriggerPredicate = predicate;
            return this;
        }

        /*
         * Events.
         */

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
        internal T1                     TriggerValue01   { get; private set; }
        internal T2                     TriggerValue02   { get; private set; }
        internal T3                     TriggerValue03   { get; private set; }
        internal bool                   TriggerValuesSet { get; private set; }
        internal Func<T1, T2, T3, bool> TriggerPredicate { get; private set; }

        public CommandBinding(EventBase type, CommandBinder binder) : base(type, binder)
        {
        }

        public ICommandBinding<T1, T2, T3> And(Event<T1, T2, T3> @event)  { return new CommandBindingComposite<T1, T2, T3>(this).And(@event); }
        public ICommandBinding<T1, T2, T3> Bind(Event<T1, T2, T3> @event) { return new CommandBindingComposite<T1, T2, T3>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public ICommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public ICommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public ICommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand>(int param) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand>(float param) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand>(bool param) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand, TCP1>(TCP1 param) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public ICommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public ICommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public ICommandBinding<T1, T2, T3> TriggerValues(T1 value01, T2 value02, T3 value03)
        {
            TriggerValue01 = value01;
            TriggerValue02 = value02;
            TriggerValue03 = value03;
            TriggerValuesSet = true;
            return this;
        }

        public ICommandBinding<T1, T2, T3> TriggerCondition(Func<T1, T2, T3, bool> predicate)
        {
            TriggerPredicate = predicate;
            return this;
        }

        /*
         * Events.
         */

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