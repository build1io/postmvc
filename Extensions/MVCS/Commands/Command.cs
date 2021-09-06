using System;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class Command : CommandBase, ICommand
    {
        public void Setup(int index)
        {
            Index = index;
            
            IsRetained = false;
            IsFailed = false;
        }

        public override void Reset()
        {
            Index = default;
        }

        public abstract void Execute();

        protected override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }

        protected override void Fail(Exception exception)
        {
            IsRetained = false;
            IsFailed = true;
            CommandBinder.FailCommand(this, exception);
        }
    }

    public abstract class Command<T1> : CommandBase, ICommand<T1>
    {
        public T1 Param01 { get; protected set; }

        public void Setup(int index, T1 param01)
        {
            Index = index;
            
            IsRetained = false;
            IsFailed = false;
            
            Param01 = param01;
        }
        
        public override void Reset()
        {
            Index = default;
            
            Param01 = default;
        }
        
        public abstract void Execute(T1 param01);

        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }

        protected override void Fail(Exception exception)
        {
            IsRetained = false;
            IsFailed = true;
            CommandBinder.FailCommand(this, exception);
        }
    }

    public abstract class Command<T1, T2> : CommandBase, ICommand<T1, T2>
    {
        public T1 Param01 { get; protected set; }
        public T2 Param02 { get; protected set; }

        public void Setup(int index, T1 param01, T2 param02)
        {
            Index = index;
            
            IsRetained = false;
            IsFailed = false;
            
            Param01 = param01;
            Param02 = param02;
        }
        
        public override void Reset()
        {
            Index = default;
            
            Param01 = default;
            Param02 = default;
        }

        public abstract void Execute(T1 param01, T2 param02);

        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }

        protected override void Fail(Exception exception)
        {
            IsRetained = false;
            IsFailed = true;
            CommandBinder.FailCommand(this, exception);
        }
    }

    public abstract class Command<T1, T2, T3> : CommandBase, ICommand<T1, T2, T3>
    {
        public T1 Param01 { get; protected set; }
        public T2 Param02 { get; protected set; }
        public T3 Param03 { get; protected set; }

        public void Setup(int index, T1 param01, T2 param02, T3 param03)
        {
            Index = index;
            
            IsRetained = false;
            IsFailed = false;
            
            Param01 = param01;
            Param02 = param02;
            Param03 = param03;
        }
        
        public override void Reset()
        {
            Index = default;
            
            Param01 = default;
            Param02 = default;
            Param03 = default;
        }

        public abstract void Execute(T1 param01, T2 param02, T3 param03);

        protected sealed override void Release()
        {
            IsRetained = false;
            CommandBinder.ReleaseCommand(this);
        }

        protected override void Fail(Exception exception)
        {
            IsRetained = false;
            IsFailed = true;
            CommandBinder.FailCommand(this, exception);
        }
    }
}