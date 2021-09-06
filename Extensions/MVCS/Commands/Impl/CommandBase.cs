using System;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    public abstract class CommandBase : ICommandBase
    {
        private static int Id;

        public    int            Index         { get; protected set; }
        protected ICommandBinder CommandBinder { get; private set; }
        public    bool           IsRetained    { get; protected set; }
        public    bool           IsFailed      { get; protected set; }

        private readonly int _id;

        protected CommandBase()
        {
            _id = ++Id;
        }

        public void SetCommandBinder(ICommandBinder commandBinder)
        {
            CommandBinder = commandBinder;
        }

        public abstract void Reset();

        protected void Retain()
        {
            IsRetained = true;
        }

        protected abstract void Release();
        protected abstract void Fail(Exception exception);

        /*
         * Dictionary Optimizations.
         */

        public override int  GetHashCode()           { return _id; }
        public override bool Equals(object obj)      { return Equals(obj as CommandBase); }
        public          bool Equals(CommandBase obj) { return obj != null && obj._id == _id; }
    }
}