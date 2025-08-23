// @formatter:off

using System;
using System.Collections.Generic;
using System.Linq;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class CommandBinding : CommandBindingBase
    {
        private Func<bool>       _triggerPredicate;
        private List<Func<bool>> _triggerPredicates;

        internal CommandBinding(EventBase type, CommandBinder binder, Pool<ICommandParams> paramsPool) : base(type, binder, paramsPool) { }

        public CommandBindingComposite And(Event @event)  { return new CommandBindingComposite(this).And(@event); }
        public CommandBindingComposite Bind(Event @event) { return new CommandBindingComposite(this).Bind(@event); }

        /*
         * Params 0.
         */

        public CommandBinding To<TCommand>()  where TCommand : Command, new() { AddCommand<TCommand>(); return this; }
        public CommandBinding To0<TCommand>() where TCommand : Command, new() { AddCommand<TCommand>(); return this; }

        /*
         * 1 params.
         */

        public CommandBinding To1<TCommand>(int param01)       where TCommand : Command<int>, new()       { AddCommand<TCommand, int>(param01);       return this; }
        public CommandBinding To1<TCommand>(float param01)     where TCommand : Command<float>, new()     { AddCommand<TCommand, float>(param01);     return this; }
        public CommandBinding To1<TCommand>(bool param01)      where TCommand : Command<bool>, new()      { AddCommand<TCommand, bool>(param01);      return this; }
        public CommandBinding To1<TCommand>(string param01)    where TCommand : Command<string>, new()    { AddCommand<TCommand, string>(param01);    return this; }
        public CommandBinding To1<TCommand>(Event param01)     where TCommand : Command<Event>, new()     { AddCommand<TCommand, Event>(param01);     return this; }
        public CommandBinding To1<TCommand>(Exception param01) where TCommand : Command<Exception>, new() { AddCommand<TCommand, Exception>(param01); return this; }
        
        public CommandBinding To1<TCommand>(Func<int> param01)       where TCommand : Command<int>, new()    { AddCommand<TCommand, int>(param01);    return this; }
        public CommandBinding To1<TCommand>(Func<float> param01)     where TCommand : Command<float>, new()  { AddCommand<TCommand, float>(param01);  return this; }
        public CommandBinding To1<TCommand>(Func<bool> param01)      where TCommand : Command<bool>, new()   { AddCommand<TCommand, bool>(param01);   return this; }
        public CommandBinding To1<TCommand>(Func<string> param01)    where TCommand : Command<string>, new() { AddCommand<TCommand, string>(param01); return this; }
        
        public CommandBinding To1<TCommand, TCP1>(TCP1 param01)       where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }
        public CommandBinding To1<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }
        
        /*
         * 2 param.
         */

        public CommandBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public CommandBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public CommandBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public CommandBinding To2<TCommand, TCP1>(TCP1 param01, string param02) where TCommand : Command<TCP1, string>, new()
        {
            AddCommand<TCommand, TCP1, string>(param01, param02);
            return this;
        }

        public CommandBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * 3 param.
         */

        public CommandBinding To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02, string param03) where TCommand : Command<TCP1, TCP2, string>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, string>(param01, param02, param03);
            return this;
        }

        public CommandBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public CommandBinding TriggerCondition(Func<bool> predicate)
        {
            if (_triggerPredicate == null)
            {
                _triggerPredicate = predicate;
            }
            else
            {
                if (_triggerPredicates == null)
                {
                    _triggerPredicates = new List<Func<bool>>(2)
                    {
                        _triggerPredicate,
                        predicate
                    };
                }
                else
                {
                    _triggerPredicates.Add(predicate);
                }
            }

            return this;
        }

        internal bool CheckTriggerCondition()
        {
            if (_triggerPredicates != null)
                return _triggerPredicates.All(predicate => predicate());
            return _triggerPredicate == null || _triggerPredicate();
        }

        /*
         * Events.
         */

        public CommandBinding OnComplete(Event @event)        { CompleteEvent = @event; return this; }
        public CommandBinding OnBreak(Event @event)           { BreakEvent = @event;    return this; }
        public CommandBinding OnFail(Event<Exception> @event) { FailEvent = @event;     return this; }
        public CommandBinding OnFail(Event @event)            { FailEvent = @event;     return this; }

        /*
         * Other.
         */

        public CommandBinding InParallel()                    { IsSequence = false;                  return this; }
        public CommandBinding InSequence()                    { IsSequence = true;                   return this; }
        public CommandBinding Once()                          { OnceBehavior = OnceBehavior.Default; return this; }
        public CommandBinding Once(OnceBehavior behavior)     { OnceBehavior = behavior;             return this; }
        public CommandBinding UnbindOnQuit()                  { IsUnbindOnQuit = true;               return this; }
        public CommandBinding Out(out CommandBinding binding) { binding = this;                      return this; }
    }

    public sealed class CommandBinding<T1> : CommandBindingBase
    {
        private T1                   _triggerValue01;
        private bool                 _triggerValuesSet;
        private Func<T1, bool>       _triggerPredicate;
        private List<Func<T1, bool>> _triggerPredicates;

        internal CommandBinding(EventBase type, CommandBinder binder, Pool<ICommandParams> paramsPool) : base(type, binder, paramsPool) { }

        public CommandBindingComposite<T1> And(Event<T1> @event)  { return new CommandBindingComposite<T1>(this).And(@event); }
        public CommandBindingComposite<T1> Bind(Event<T1> @event) { return new CommandBindingComposite<T1>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public CommandBinding<T1> To0<TCommand>() where TCommand : Command, new() { AddCommand<TCommand>(); return this; }

        /*
         * Params 1.
         */

        public CommandBinding<T1> To<TCommand>()                where TCommand : Command<T1>, new()        { AddCommand<TCommand>();                 return this; }
        public CommandBinding<T1> To<TCommand>(int param)       where TCommand : Command<int>, new()       { AddCommand<TCommand, int>(param);       return this; }
        public CommandBinding<T1> To<TCommand>(float param)     where TCommand : Command<float>, new()     { AddCommand<TCommand, float>(param);     return this; }
        public CommandBinding<T1> To<TCommand>(bool param)      where TCommand : Command<bool>, new()      { AddCommand<TCommand, bool>(param);      return this; }
        public CommandBinding<T1> To<TCommand>(string param)    where TCommand : Command<string>, new()    { AddCommand<TCommand, string>(param);    return this; }
        public CommandBinding<T1> To<TCommand>(Event param)     where TCommand : Command<Event>, new()     { AddCommand<TCommand, Event>(param);     return this; }
        public CommandBinding<T1> To<TCommand>(Exception param) where TCommand : Command<Exception>, new() { AddCommand<TCommand, Exception>(param); return this; }
        
        public CommandBinding<T1> To<TCommand>(Func<int> param)       where TCommand : Command<int>, new()       { AddCommand<TCommand, int>(param);       return this; }
        public CommandBinding<T1> To<TCommand>(Func<float> param)     where TCommand : Command<float>, new()     { AddCommand<TCommand, float>(param);     return this; }
        public CommandBinding<T1> To<TCommand>(Func<bool> param)      where TCommand : Command<bool>, new()      { AddCommand<TCommand, bool>(param);      return this; }
        public CommandBinding<T1> To<TCommand>(Func<string> param)    where TCommand : Command<string>, new()    { AddCommand<TCommand, string>(param);    return this; }
        
        public CommandBinding<T1> To<TCommand, TCP1>(TCP1 param01)       where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }
        public CommandBinding<T1> To<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }

        public CommandBinding<T1> To1<TCommand>()                where TCommand : Command<T1>, new()        { AddCommand<TCommand>();                 return this; }
        public CommandBinding<T1> To1<TCommand>(int param)       where TCommand : Command<int>, new()       { AddCommand<TCommand, int>(param);       return this; }
        public CommandBinding<T1> To1<TCommand>(float param)     where TCommand : Command<float>, new()     { AddCommand<TCommand, float>(param);     return this; }
        public CommandBinding<T1> To1<TCommand>(bool param)      where TCommand : Command<bool>, new()      { AddCommand<TCommand, bool>(param);      return this; }
        public CommandBinding<T1> To1<TCommand>(string param)    where TCommand : Command<string>, new()    { AddCommand<TCommand, string>(param);    return this; }
        public CommandBinding<T1> To1<TCommand>(Event param)     where TCommand : Command<Event>, new()     { AddCommand<TCommand, Event>(param);     return this; }
        public CommandBinding<T1> To1<TCommand>(Exception param) where TCommand : Command<Exception>, new() { AddCommand<TCommand, Exception>(param); return this; }

        public CommandBinding<T1> To1<TCommand>(Func<int> param)       where TCommand : Command<int>, new()       { AddCommand<TCommand, int>(param);       return this; }
        public CommandBinding<T1> To1<TCommand>(Func<float> param)     where TCommand : Command<float>, new()     { AddCommand<TCommand, float>(param);     return this; }
        public CommandBinding<T1> To1<TCommand>(Func<bool> param)      where TCommand : Command<bool>, new()      { AddCommand<TCommand, bool>(param);      return this; }
        public CommandBinding<T1> To1<TCommand>(Func<string> param)    where TCommand : Command<string>, new()    { AddCommand<TCommand, string>(param);    return this; }
        
        public CommandBinding<T1> To1<TCommand, TCP1>(TCP1 param01)       where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }
        public CommandBinding<T1> To1<TCommand, TCP1>(Func<TCP1> param01) where TCommand : Command<TCP1>, new() { AddCommand<TCommand, TCP1>(param01); return this; }

        /*
         * Params 2.
         */

        public CommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public CommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public CommandBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public CommandBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public CommandBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1> To3<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<T1, TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }
        
        public CommandBinding<T1> To3<TCommand, TCP1>(TCP1 param01, string param02) where TCommand : Command<T1, TCP1, string>, new()
        {
            AddCommand<TCommand, TCP1, string>(param01, param02);
            return this;
        }

        public CommandBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public CommandBinding<T1> TriggerCondition(T1 value01)
        {
            _triggerValue01 = value01;
            _triggerValuesSet = true;
            return this;
        }

        public CommandBinding<T1> TriggerCondition(Func<T1, bool> predicate)
        {
            if (_triggerPredicate == null)
            {
                _triggerPredicate = predicate;
            }
            else
            {
                if (_triggerPredicates == null)
                {
                    _triggerPredicates = new List<Func<T1, bool>>(2)
                    {
                        _triggerPredicate,
                        predicate
                    };
                }
                else
                {
                    _triggerPredicates.Add(predicate);
                }
            }

            return this;
        }

        internal bool CheckTriggerCondition(T1 param01)
        {
            if (_triggerValuesSet && !EqualityComparer<T1>.Default.Equals(_triggerValue01, param01))
                return false;

            if (_triggerPredicates != null)
            {
                foreach (var predicate in _triggerPredicates)
                {
                    if (!predicate(param01)) 
                        return false;
                }

                return true;
            }
            
            return _triggerPredicate == null || _triggerPredicate(param01);
        }

        /*
         * Events.
         */

        public CommandBinding<T1> OnComplete(Event<T1> @event)    { CompleteEvent = @event; return this; }
        public CommandBinding<T1> OnComplete(Event @event)        { CompleteEvent = @event; return this; }
        public CommandBinding<T1> OnBreak(Event<T1> @event)       { BreakEvent = @event;    return this; }
        public CommandBinding<T1> OnBreak(Event @event)           { BreakEvent = @event;    return this; }
        public CommandBinding<T1> OnFail(Event<Exception> @event) { FailEvent = @event;     return this; }
        public CommandBinding<T1> OnFail(Event @event)            { FailEvent = @event;     return this; }

        /*
         * Other.
         */

        public CommandBinding<T1> InParallel()                        { IsSequence = false;                  return this; }
        public CommandBinding<T1> InSequence()                        { IsSequence = true;                   return this; }
        public CommandBinding<T1> Once()                              { OnceBehavior = OnceBehavior.Default; return this; }
        public CommandBinding<T1> Once(OnceBehavior behavior)         { OnceBehavior = behavior;             return this; }
        public CommandBinding<T1> UnbindOnQuit()                      { IsUnbindOnQuit = true;               return this; }
        public CommandBinding<T1> Out(out CommandBinding<T1> binding) { binding = this;                      return this; }
    }

    public sealed class CommandBinding<T1, T2> : CommandBindingBase
    {
        private T1                       _triggerValue01;
        private T2                       _triggerValue02;
        private bool                     _triggerValuesSet;
        private Func<T1, T2, bool>       _triggerPredicate;
        private List<Func<T1, T2, bool>> _triggerPredicates;

        internal CommandBinding(EventBase type, CommandBinder binder, Pool<ICommandParams> paramsPool) : base(type, binder, paramsPool) { }

        public CommandBindingComposite<T1, T2> And(Event<T1, T2> @event)  { return new CommandBindingComposite<T1, T2>(this).And(@event); }
        public CommandBindingComposite<T1, T2> Bind(Event<T1, T2> @event) { return new CommandBindingComposite<T1, T2>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public CommandBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public CommandBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public CommandBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public CommandBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public CommandBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public CommandBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public CommandBinding<T1, T2> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1, T2> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public CommandBinding<T1, T2> TriggerCondition(T1 value01, T2 value02)
        {
            _triggerValue01 = value01;
            _triggerValue02 = value02;
            _triggerValuesSet = true;
            return this;
        }

        public CommandBinding<T1, T2> TriggerCondition(Func<T1, T2, bool> predicate)
        {
            if (_triggerPredicate == null)
            {
                _triggerPredicate = predicate;
            }
            else
            {
                if (_triggerPredicates == null)
                {
                    _triggerPredicates = new List<Func<T1, T2, bool>>(2)
                    {
                        _triggerPredicate,
                        predicate
                    };
                }
                else
                {
                    _triggerPredicates.Add(predicate);
                }
            }

            return this;
        }

        internal bool CheckTriggerCondition(T1 param01, T2 param02)
        {
            if (_triggerValuesSet &&
                (!EqualityComparer<T1>.Default.Equals(_triggerValue01, param01) ||
                 !EqualityComparer<T2>.Default.Equals(_triggerValue02, param02)))
                return false;

            if (_triggerPredicates != null)
            {
                foreach (var predicate in _triggerPredicates)
                {
                    if (!predicate(param01, param02)) 
                        return false;
                }

                return true;
            }

            return _triggerPredicate == null || _triggerPredicate(param01, param02);
        }

        /*
         * Events.
         */

        public CommandBinding<T1, T2> OnComplete(Event<T1, T2> @event) { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2> OnComplete(Event<T1> @event)     { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2> OnComplete(Event @event)         { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2> OnBreak(Event<T1, T2> @event)    { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2> OnBreak(Event<T1> @event)        { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2> OnBreak(Event @event)            { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2> OnFail(Event<Exception> @event)  { FailEvent = @event;     return this; }
        public CommandBinding<T1, T2> OnFail(Event @event)             { FailEvent = @event;     return this; }

        /*
         * Other.
         */

        public CommandBinding<T1, T2> InParallel()                            { IsSequence = false;                  return this; }
        public CommandBinding<T1, T2> InSequence()                            { IsSequence = true;                   return this; }
        public CommandBinding<T1, T2> Once()                                  { OnceBehavior = OnceBehavior.Default; return this; }
        public CommandBinding<T1, T2> Once(OnceBehavior behavior)             { OnceBehavior = behavior;             return this; }
        public CommandBinding<T1, T2> UnbindOnQuit()                          { IsUnbindOnQuit = true;               return this; }
        public CommandBinding<T1, T2> Out(out CommandBinding<T1, T2> binding) { binding = this;                      return this; }
    }

    public sealed class CommandBinding<T1, T2, T3> : CommandBindingBase
    {
        private T1                           _triggerValue01;
        private T2                           _triggerValue02;
        private T3                           _triggerValue03;
        private bool                         _triggerValuesSet;
        private Func<T1, T2, T3, bool>       _triggerPredicate;
        private List<Func<T1, T2, T3, bool>> _triggerPredicates;

        internal CommandBinding(EventBase type, CommandBinder binder, Pool<ICommandParams> paramsPool) : base(type, binder, paramsPool) { }

        public CommandBindingComposite<T1, T2, T3> And(Event<T1, T2, T3> @event)  { return new CommandBindingComposite<T1, T2, T3>(this).And(@event); }
        public CommandBindingComposite<T1, T2, T3> Bind(Event<T1, T2, T3> @event) { return new CommandBindingComposite<T1, T2, T3>(this).Bind(@event); }

        /*
         * Params 0.
         */

        public CommandBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public CommandBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public CommandBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand>(int param) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand>(float param) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand>(bool param) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand, TCP1>(TCP1 param) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param);
            return this;
        }

        public CommandBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public CommandBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1, T2, T3> To<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public CommandBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Triggering.
         */

        public CommandBinding<T1, T2, T3> TriggerCondition(T1 value01, T2 value02, T3 value03)
        {
            _triggerValue01 = value01;
            _triggerValue02 = value02;
            _triggerValue03 = value03;
            _triggerValuesSet = true;
            return this;
        }

        public CommandBinding<T1, T2, T3> TriggerCondition(Func<T1, T2, T3, bool> predicate)
        {
            if (_triggerPredicate == null)
            {
                _triggerPredicate = predicate;
            }
            else
            {
                if (_triggerPredicates == null)
                {
                    _triggerPredicates = new List<Func<T1, T2, T3, bool>>(2)
                    {
                        _triggerPredicate,
                        predicate
                    };
                }
                else
                {
                    _triggerPredicates.Add(predicate);
                }
            }

            return this;
        }

        internal bool CheckTriggerCondition(T1 param01, T2 param02, T3 param03)
        {
            if (_triggerValuesSet &&
                (!EqualityComparer<T1>.Default.Equals(_triggerValue01, param01) ||
                 !EqualityComparer<T2>.Default.Equals(_triggerValue02, param02) ||
                 !EqualityComparer<T3>.Default.Equals(_triggerValue03, param03)))
                return false;

            if (_triggerPredicates != null)
            {
                foreach (var predicate in _triggerPredicates)
                {
                    if (!predicate(param01, param02, param03)) 
                        return false;
                }
                return true;
            }

            return _triggerPredicate == null || _triggerPredicate(param01, param02, param03);
        }

        /*
         * Events.
         */

        public CommandBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event) { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)     { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2, T3> OnComplete(Event<T1> @event)         { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2, T3> OnComplete(Event @event)             { CompleteEvent = @event; return this; }
        public CommandBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event)    { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)        { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2, T3> OnBreak(Event<T1> @event)            { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2, T3> OnBreak(Event @event)                { BreakEvent = @event;    return this; }
        public CommandBinding<T1, T2, T3> OnFail(Event<Exception> @event)      { FailEvent = @event;     return this; }
        public CommandBinding<T1, T2, T3> OnFail(Event @event)                 { FailEvent = @event;     return this; }

        /*
         * Other.
         */

        public CommandBinding<T1, T2, T3> InParallel()                                { IsSequence = false;                  return this; }
        public CommandBinding<T1, T2, T3> InSequence()                                { IsSequence = true;                   return this; }
        public CommandBinding<T1, T2, T3> Once()                                      { OnceBehavior = OnceBehavior.Default; return this; }
        public CommandBinding<T1, T2, T3> Once(OnceBehavior behavior)                 { OnceBehavior = behavior;             return this; }
        public CommandBinding<T1, T2, T3> UnbindOnQuit()                              { IsUnbindOnQuit = true;               return this; }
        public CommandBinding<T1, T2, T3> Out(out CommandBinding<T1, T2, T3> binding) { binding = this;                      return this; }
    }
}

// @formatter:on