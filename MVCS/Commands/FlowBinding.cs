using System;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public sealed class FlowBinding : CommandBindingBase, IFlowBinding
    {
        internal FlowBinding(CommandBinder binder, Pool<CommandParamsBase> paramsPool) : base(null, binder, paramsPool)
        {
        }

        /*
         * Params 0.
         */

        public IFlowBinding To<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * 1 params.
         */

        public IFlowBinding To1<TCommand>(int param01) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding To1<TCommand>(float param01) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding To1<TCommand>(bool param01) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding To1<TCommand>(string param01) where TCommand : Command<string>, new()
        {
            AddCommand<TCommand, string>(param01);
            return this;
        }

        public IFlowBinding To1<TCommand>(Exception param01) where TCommand : Command<Exception>, new()
        {
            AddCommand<TCommand, Exception>(param01);
            return this;
        }

        public IFlowBinding To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * 2 param.
         */

        public IFlowBinding To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public IFlowBinding To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public IFlowBinding To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public IFlowBinding To2<TCommand, TCP1>(TCP1 param01, string param02) where TCommand : Command<TCP1, string>, new()
        {
            AddCommand<TCommand, TCP1, string>(param01, param02);
            return this;
        }

        public IFlowBinding To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * 3 param.
         */

        public IFlowBinding To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02, string param03) where TCommand : Command<TCP1, TCP2, string>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, string>(param01, param02, param03);
            return this;
        }

        public IFlowBinding To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */

        public IFlowBinding OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public IFlowBinding OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }
        
        /*
         * Other.
         */

        public IFlowBinding InParallel()
        {
            IsSequence = false;
            return this;
        }

        public IFlowBinding InSequence()
        {
            IsSequence = true;
            return this;
        }

        public IFlowBinding Execute()
        {
            CommandBinder.ProcessFlow(this);
            return this;
        }

        public bool Break()
        {
            if (!IsExecuting)
                return false;
            
            RegisterBreak();
            return true;
        }
    }
    
    public sealed class FlowBinding<T1> : CommandBindingBase, IFlowBinding<T1>
    {
        internal FlowBinding(CommandBinder binder, Pool<CommandParamsBase> paramsPool) : base(null, binder, paramsPool)
        {
        }

        /*
         * Params 0.
         */

        public IFlowBinding<T1> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public IFlowBinding<T1> To<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1> To<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public IFlowBinding<T1> To<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public IFlowBinding<T1> To<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public IFlowBinding<T1> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public IFlowBinding<T1> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public IFlowBinding<T1> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public IFlowBinding<T1> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, int param02) where TCommand : Command<TCP1, int>, new()
        {
            AddCommand<TCommand, TCP1, int>(param01, param02);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, float param02) where TCommand : Command<TCP1, float>, new()
        {
            AddCommand<TCommand, TCP1, float>(param01, param02);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public IFlowBinding<T1> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public IFlowBinding<T1> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1> To3<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<T1, TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        public IFlowBinding<T1> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */

        public IFlowBinding<T1> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1> OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public IFlowBinding<T1> OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }
        
        /*
         * Other.
         */

        public IFlowBinding<T1> InParallel()
        {
            IsSequence = false;
            return this;
        }

        public IFlowBinding<T1> InSequence()
        {
            IsSequence = true;
            return this;
        }

        public IFlowBinding<T1> Execute(T1 param01)
        {
            CommandBinder.ProcessFlow(this, param01);
            return this;
        }

        public bool Break()
        {
            if (!IsExecuting)
                return false;
            
            RegisterBreak();
            return true;
        }
    }
    
    public sealed class FlowBinding<T1, T2> : CommandBindingBase, IFlowBinding<T1, T2>
    {
        internal FlowBinding(CommandBinder binder, Pool<CommandParamsBase> paramsPool) : base(null, binder, paramsPool)
        {
        }

        /*
         * Params 0.
         */

        public IFlowBinding<T1, T2> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public IFlowBinding<T1, T2> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public IFlowBinding<T1, T2> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public IFlowBinding<T1, T2> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public IFlowBinding<T1, T2> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public IFlowBinding<T1, T2> To<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand>(int param01) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand>(float param01) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand>(bool param01) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1, T2> To2<TCommand, TCP1>(TCP1 param01, bool param02) where TCommand : Command<TCP1, bool>, new()
        {
            AddCommand<TCommand, TCP1, bool>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public IFlowBinding<T1, T2> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1, T2> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1, T2> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */

        public IFlowBinding<T1, T2> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2> OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }

        /*
         * Other.
         */

        public IFlowBinding<T1, T2> InParallel()
        {
            IsSequence = false;
            return this;
        }

        public IFlowBinding<T1, T2> InSequence()
        {
            IsSequence = true;
            return this;
        }

        public IFlowBinding<T1, T2> Execute(T1 param01, T2 param02)
        {
            CommandBinder.ProcessFlow(this, param01, param02);
            return this;
        }

        public bool Break()
        {
            if (!IsExecuting)
                return false;
            
            RegisterBreak();
            return true;
        }
    }
    
    public sealed class FlowBinding<T1, T2, T3> : CommandBindingBase, IFlowBinding<T1, T2, T3>
    {
        internal FlowBinding(CommandBinder binder, Pool<CommandParamsBase> paramsPool) : base(null, binder, paramsPool)
        {
        }

        /*
         * Params 0.
         */

        public IFlowBinding<T1, T2, T3> To0<TCommand>() where TCommand : Command, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        /*
         * Params 1.
         */

        public IFlowBinding<T1, T2, T3> To1<TCommand>() where TCommand : Command<T1>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2, T3> To1<TCommand>(int param) where TCommand : Command<int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To1<TCommand>(float param) where TCommand : Command<float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To1<TCommand>(bool param) where TCommand : Command<bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To1<TCommand, TCP1>(TCP1 param01) where TCommand : Command<TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        /*
         * Params 2.
         */

        public IFlowBinding<T1, T2, T3> To2<TCommand>() where TCommand : Command<T1, T2>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2, T3> To2<TCommand>(int param) where TCommand : Command<T1, int>, new()
        {
            AddCommand<TCommand, int>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To2<TCommand>(float param) where TCommand : Command<T1, float>, new()
        {
            AddCommand<TCommand, float>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To2<TCommand>(bool param) where TCommand : Command<T1, bool>, new()
        {
            AddCommand<TCommand, bool>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To2<TCommand, TCP1>(TCP1 param) where TCommand : Command<T1, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To2<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        /*
         * Params 3.
         */

        public IFlowBinding<T1, T2, T3> To<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand>() where TCommand : Command<T1, T2, T3>, new()
        {
            AddCommand<TCommand>();
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand>(int param01) where TCommand : Command<T1, T2, int>, new()
        {
            AddCommand<TCommand, int>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand>(float param01) where TCommand : Command<T1, T2, float>, new()
        {
            AddCommand<TCommand, float>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand>(bool param01) where TCommand : Command<T1, T2, bool>, new()
        {
            AddCommand<TCommand, bool>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand, TCP1>(TCP1 param01) where TCommand : Command<T1, T2, TCP1>, new()
        {
            AddCommand<TCommand, TCP1>(param01);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2>(TCP1 param01, TCP2 param02) where TCommand : Command<T1, TCP1, TCP2>, new()
        {
            AddCommand<TCommand, TCP1, TCP2>(param01, param02);
            return this;
        }

        public IFlowBinding<T1, T2, T3> To3<TCommand, TCP1, TCP2, TCP3>(TCP1 param01, TCP2 param02, TCP3 param03) where TCommand : Command<TCP1, TCP2, TCP3>, new()
        {
            AddCommand<TCommand, TCP1, TCP2, TCP3>(param01, param02, param03);
            return this;
        }

        /*
         * Events.
         */

        public IFlowBinding<T1, T2, T3> OnComplete(Event<T1, T2, T3> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnComplete(Event<T1, T2> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnComplete(Event<T1> @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnComplete(Event @event)
        {
            CompleteEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnBreak(Event<T1, T2, T3> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnBreak(Event<T1, T2> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnBreak(Event<T1> @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnBreak(Event @event)
        {
            BreakEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnFail(Event<Exception> @event)
        {
            FailEvent = @event;
            return this;
        }

        public IFlowBinding<T1, T2, T3> OnFail(Event @event)
        {
            FailEvent = @event;
            return this;
        }

        /*
         * Other.
         */

        public IFlowBinding<T1, T2, T3> InParallel()
        {
            IsSequence = false;
            return this;
        }

        public IFlowBinding<T1, T2, T3> InSequence()
        {
            IsSequence = true;
            return this;
        }

        public IFlowBinding<T1, T2, T3> Execute(T1 param01, T2 param02, T3 param03)
        {
            CommandBinder.ProcessFlow(this, param01, param02, param03);
            return this;
        }

        public bool Break()
        {
            if (!IsExecuting)
                return false;
            
            RegisterBreak();
            return true;
        }
    }
}