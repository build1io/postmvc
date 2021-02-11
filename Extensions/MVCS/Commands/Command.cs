using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class Command : CommandBase, ICommand
    {
        public abstract void Execute();

        protected override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }
    }

    public abstract class Command<T1> : CommandBase, ICommand<T1>
    {
        public T1 Param01 { get; protected set; }

        public void SetData(T1 param01)
        {
            Param01 = param01;
        }
        
        public abstract void Execute(T1 param01);
        
        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }
    }

    public abstract class Command<T1, T2> : CommandBase, ICommand<T1, T2>
    {
        public T1 Param01 { get; protected set; }
        public T2 Param02 { get; protected set; }

        public void SetData(T1 param01, T2 param02)
        {
            Param01 = param01;
            Param02 = param02;
        }

        public abstract void Execute(T1 param01, T2 param02);
        
        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }
    }

    public abstract class Command<T1, T2, T3> : CommandBase, ICommand<T1, T2, T3>
    {
        public T1 Param01 { get; protected set; }
        public T2 Param02 { get; protected set; }
        public T3 Param03 { get; protected set; }

        public void SetData(T1 param01, T2 param02, T3 param03)
        {
            Param01 = param01;
            Param02 = param02;
            Param03 = param03;
        }
        
        public abstract void Execute(T1 param01, T2 param02, T3 param03);
        
        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }
    }
}