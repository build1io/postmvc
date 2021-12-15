using System;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class Command : CommandBase, ICommand
    {
        public CommandBinding Binding { get; private set; }

        public void PreExecute(CommandBinding binding)
        {
            Binding = binding;
        }

        public abstract void Execute();

        protected override void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            CommandBinder.OnCommandFinish(this);
        }

        protected override void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            IsBreak = true;
            CommandBinder.OnCommandFinish(this);
        }

        protected override void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand);
            
            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;
            CommandBinder.OnCommandFail(this, exception);
        }
    }

    public abstract class Command<T1> : CommandBase, ICommand<T1>
    {
        public CommandBinding<T1> Binding { get; private set; }
        public T1                 Param01 { get; protected set; }

        public void PreExecute(CommandBinding<T1> binding, T1 param01)
        {
            Binding = binding;
            Param01 = param01;
        }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
        }

        public abstract void Execute(T1 param01);

        protected sealed override void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            CommandBinder.OnCommandFinish(this);
        }

        protected override void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            IsBreak = true;
            CommandBinder.OnCommandFinish(this);
        }
        
        protected override void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand);
            
            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;
            CommandBinder.OnCommandFail(this, exception);
        }
    }

    public abstract class Command<T1, T2> : CommandBase, ICommand<T1, T2>
    {
        public CommandBinding<T1, T2> Binding { get; private set; }
        public T1                     Param01 { get; protected set; }
        public T2                     Param02 { get; protected set; }

        public void PreExecute(CommandBinding<T1, T2> binding, T1 param01, T2 param02)
        {
            Binding = binding;
            Param01 = param01;
            Param02 = param02;
        }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
        }

        public abstract void Execute(T1 param01, T2 param02);

        protected sealed override void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            CommandBinder.OnCommandFinish(this);
        }
        
        protected override void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            IsBreak = true;
            CommandBinder.OnCommandFinish(this);
        }

        protected override void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand);
            
            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;
            CommandBinder.OnCommandFail(this, exception);
        }
    }

    public abstract class Command<T1, T2, T3> : CommandBase, ICommand<T1, T2, T3>
    {
        public CommandBinding<T1, T2, T3> Binding { get; private set; }
        public T1                         Param01 { get; protected set; }
        public T2                         Param02 { get; protected set; }
        public T3                         Param03 { get; protected set; }

        public void PreExecute(CommandBinding<T1, T2, T3> binding, T1 param01, T2 param02, T3 param03)
        {
            Binding = binding;
            Param01 = param01;
            Param02 = param02;
            Param03 = param03;
        }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
            Param03 = default;
        }

        public abstract void Execute(T1 param01, T2 param02, T3 param03);

        protected sealed override void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            CommandBinder.OnCommandFinish(this);
        }
        
        protected override void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);
            
            IsResolved = true;
            IsRetained = false;
            IsBreak = true;
            CommandBinder.OnCommandFinish(this);
        }

        protected override void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand);
            
            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;
            CommandBinder.OnCommandFail(this, exception);
        }
    }
}