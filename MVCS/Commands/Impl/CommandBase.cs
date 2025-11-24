using System;

namespace Build1.PostMVC.Core.MVCS.Commands.Impl
{
    public abstract class CommandBase
    {
        private const int DefaultIndex = -1;

        private static int Id;

        internal int                Index      { get; private set; } = DefaultIndex;
        internal CommandBindingBase Binding    { get; private set; }
        internal ICommandParams     Params     { get; private set; }
        internal Exception          Exception  { get; set; }
        internal bool               IsExecuted { get; private set; }
        internal bool               IsRetained { get; set; }
        internal bool               IsBreak    { get; set; }
        internal bool               IsFailed   { get; set; }
        internal bool               IsClean    => Index == DefaultIndex;

        internal bool IsResolved { get; set; }

        protected readonly int id;

        private CommandBinder _commandBinder;

        protected CommandBase()
        {
            id = ++Id;
        }

        /*
         * Public.
         */

        internal void SetCommandBinder(CommandBinder commandBinder)
        {
            _commandBinder = commandBinder;
        }

        internal void Setup(int index, CommandBindingBase binding, ICommandParams param)
        {
            Index = index;
            Binding = binding;
            Params = param;
            IsResolved = false;
        }

        internal void PostExecute()
        {
            IsExecuted = true;

            if (!IsRetained)
                IsResolved = true;
        }

        internal virtual void Reset()
        {
            Index = DefaultIndex;
            Exception = default;
            IsExecuted = false;
            IsRetained = false;
            IsBreak = false;
            IsFailed = false;
        }

        /*
         * Public.
         */

        internal abstract void InternalExecute(ICommandParams param, ICommandParams paramAdditional);

        /*
         * Internal.
         */

        internal void RetainInternal()                  { Retain(); }
        internal void ReleaseInternal()                 { Release(); }
        internal void BreakInternal()                   { Break(); }
        internal void FailInternal(Exception exception) { Fail(exception); }

        /*
         * Protected.
         */

        protected void Retain()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToRetainResolvedCommand, this);

            IsRetained = true;
        }

        protected void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand, this);

            IsResolved = true;
            IsRetained = false;

            _commandBinder.OnCommandFinish(this);
        }

        protected void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToBreakResolvedCommand, this);

            IsResolved = true;
            IsRetained = false;
            IsBreak = true;

            _commandBinder.OnCommandFinish(this);
        }

        protected void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand, this);

            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;

            _commandBinder.OnCommandFail(this, exception);
        }

        protected EventHandleCommand ToHandle()
        {
            return new EventHandleCommand(this);
        }

        /*
         * Dictionary Optimizations.
         */

        public override int  GetHashCode()           { return id; }
        public override bool Equals(object obj)      { return Equals(obj as CommandBase); }
        public          bool Equals(CommandBase obj) { return obj != null && obj.id == id; }
    }
}