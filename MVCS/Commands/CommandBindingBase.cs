using System;
using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Commands.Impl;
using Build1.PostMVC.Core.MVCS.Events.Impl;
using Build1.PostMVC.Core.Utils.Pooling;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public abstract class CommandBindingBase : ICommandBindingBase
    {
        internal EventBase                          Event             { get; }
        internal CommandBinder                      CommandBinder     { get; }
        internal Pool<CommandParamsBase>            CommandParamsPool { get; }
        internal List<Type>                         Commands          { get; }
        internal Dictionary<int, CommandParamsBase> Params            { get; set; }
        internal int                                CommandsExecuted  { get; set; }
        internal int                                CommandsReleased  { get; set; }
        internal List<Exception>                    CommandsFailed    { get; set; }
        internal EventBase                          CompleteEvent     { get; set; }
        internal EventBase                          BreakEvent        { get; set; }
        internal EventBase                          FailEvent         { get; set; }
        internal bool                               IsSequence        { get; set; }
        internal OnceBehavior                       OnceBehavior      { get; set; }
        internal bool                               IsUnbindOnQuit    { get; set; }

        public   bool IsExecuting { get; private set; }
        internal bool IsBreak     { get; private set; }

        internal bool HasFails => CommandsFailed != null && CommandsFailed.Count > 0;

        private Pool<CommandParamsBase> _commandParamsPool;

        internal CommandBindingBase(EventBase type, CommandBinder binder, Pool<CommandParamsBase> paramsPool)
        {
            Event = type;
            CommandBinder = binder;
            CommandParamsPool = paramsPool;
            Commands = new List<Type>();
        }

        protected void AddCommand<TCommand>() where TCommand : CommandBase
        {
            Commands.Add(typeof(TCommand));
        }

        protected void AddCommand<TCommand, T1>(T1 param01) where TCommand : CommandBase
        {
            Commands.Add(typeof(TCommand));

            var param = CommandParamsPool.Take<CommandParams<T1>>();
            param.Param01 = param01;

            Params ??= new Dictionary<int, CommandParamsBase>();
            Params.Add(Commands.Count - 1, param);
        }

        protected void AddCommand<TCommand, T1, T2>(T1 param01, T2 param02) where TCommand : CommandBase
        {
            Commands.Add(typeof(TCommand));

            var param = CommandParamsPool.Take<CommandParams<T1, T2>>();
            param.Param01 = param01;
            param.Param02 = param02;

            Params ??= new Dictionary<int, CommandParamsBase>();
            Params.Add(Commands.Count - 1, param);
        }

        protected void AddCommand<TCommand, T1, T2, T3>(T1 param01, T2 param02, T3 param03) where TCommand : CommandBase
        {
            Commands.Add(typeof(TCommand));

            var param = CommandParamsPool.Take<CommandParams<T1, T2, T3>>();
            param.Param01 = param01;
            param.Param02 = param02;
            param.Param03 = param03;

            Params ??= new Dictionary<int, CommandParamsBase>();
            Params.Add(Commands.Count - 1, param);
        }

        /*
         * Runtime.
         */

        internal void StartExecution()
        {
            IsExecuting = true;
        }

        internal void FinishExecution()
        {
            CommandsExecuted = 0;
            CommandsReleased = 0;
            CommandsFailed = null;
            IsExecuting = false;
            IsBreak = false;
        }

        internal void RegisterCommandExecute()
        {
            CommandsExecuted++;
        }

        internal void RegisterBreak()
        {
            IsBreak = true;
        }

        internal void RegisterCommandRelease()
        {
            CommandsReleased++;
        }

        internal void RegisterCommandException(Exception exception)
        {
            CommandsFailed ??= new List<Exception>();
            CommandsFailed.Add(exception);
        }

        internal bool CheckAllExecuted()
        {
            return Commands.Count == CommandsExecuted;
        }

        internal bool CheckAllReleased()
        {
            if (IsSequence)
                return Commands.Count == CommandsReleased || HasFails;
            return Commands.Count == CommandsReleased + (CommandsFailed?.Count ?? 0);
        }

        internal void Dispose()
        {
            if (Params == null)
                return;

            foreach (var param in Params.Values)
                CommandParamsPool.Return(param);
        }
    }
}