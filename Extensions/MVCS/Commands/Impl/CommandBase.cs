using System;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public abstract class CommandBase
    {
        private const int DefaultIndex = -1;

        private static int Id;

        public int                Index      { get; private set; } = DefaultIndex;
        public CommandBindingBase Binding    { get; private set; }
        public CommandParamsBase  Params     { get; private set; }
        public Exception          Exception  { get; protected set; }
        public bool               IsExecuted { get; private set; }
        public bool               IsRetained { get; protected set; }
        public bool               IsBreak    { get; protected set; }
        public bool               IsFailed   { get; protected set; }
        public bool               IsClean    => Index == DefaultIndex;

        protected ICommandBinder CommandBinder { get; private set; }
        protected bool           IsResolved    { get; set; }

        private readonly int _id;

        protected CommandBase()
        {
            _id = ++Id;
        }

        /*
         * Public.
         */

        public void SetCommandBinder(ICommandBinder commandBinder)
        {
            CommandBinder = commandBinder;
        }

        public void Setup(int index, CommandBindingBase binding, CommandParamsBase param)
        {
            Index = index;
            Binding = binding;
            Params = param;
            IsResolved = false;
        }

        public void PostExecute()
        {
            IsExecuted = true;

            if (!IsRetained)
                IsResolved = true;
        }

        public virtual void Reset()
        {
            Index = DefaultIndex;
            Exception = default;
            IsExecuted = false;
            IsRetained = false;
            IsBreak = false;
            IsFailed = false;
        }

        /*
         * Protected.
         */

        protected void Retain()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToRetainResolvedCommand);

            IsRetained = true;
        }

        protected void Release()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);

            IsResolved = true;
            IsRetained = false;
            CommandBinder.OnCommandFinish(this);
        }

        protected void Break()
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToReleaseResolvedCommand);

            IsResolved = true;
            IsRetained = false;
            IsBreak = true;
            CommandBinder.OnCommandFinish(this);
        }

        protected void Fail(Exception exception)
        {
            if (IsResolved)
                throw new CommandException(CommandExceptionType.AttemptToFailResolvedCommand);

            IsResolved = true;
            Exception = exception;
            IsRetained = false;
            IsFailed = true;

            CommandBinder.OnCommandFail(this, exception);
        }

        /*
         * Dictionary Optimizations.
         */

        public override int  GetHashCode()           { return _id; }
        public override bool Equals(object obj)      { return Equals(obj as CommandBase); }
        public          bool Equals(CommandBase obj) { return obj != null && obj._id == _id; }
    }
}