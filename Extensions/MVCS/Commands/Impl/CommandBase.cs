using System;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public abstract class CommandBase : ICommandBase
    {
        protected const int DefaultIndex = -1;

        private static int Id;

        public int       Index      { get; private set; } = DefaultIndex;
        public Exception Exception  { get; protected set; }
        public bool      IsExecuted { get; private set; }
        public bool      IsRetained { get; protected set; }
        public bool      IsBreak    { get; protected set; }
        public bool      IsFailed   { get; protected set; }
        public bool      IsClean    => Index == DefaultIndex;

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

        public void Setup(int index)
        {
            Index = index;
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

        protected abstract void Release();
        protected abstract void Break();
        protected abstract void Fail(Exception exception);

        /*
         * Dictionary Optimizations.
         */

        public override int  GetHashCode()           { return _id; }
        public override bool Equals(object obj)      { return Equals(obj as CommandBase); }
        public          bool Equals(CommandBase obj) { return obj != null && obj._id == _id; }
    }
}