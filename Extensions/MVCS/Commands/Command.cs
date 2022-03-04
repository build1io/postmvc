using Build1.PostMVC.Extensions.MVCS.Commands.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public abstract class Command : CommandBase
    {
        public abstract void Execute();
    }

    public abstract class Command<T1> : CommandBase
    {
        public T1 Param01 { get; set; }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
        }

        public abstract void Execute(T1 param01);
    }

    public abstract class Command<T1, T2> : CommandBase
    {
        public T1 Param01 { get; set; }
        public T2 Param02 { get; set; }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
        }

        public abstract void Execute(T1 param01, T2 param02);
    }

    public abstract class Command<T1, T2, T3> : CommandBase
    {
        public T1 Param01 { get; set; }
        public T2 Param02 { get; set; }
        public T3 Param03 { get; set; }

        public override void Reset()
        {
            base.Reset();

            Param01 = default;
            Param02 = default;
            Param03 = default;
        }

        public abstract void Execute(T1 param01, T2 param02, T3 param03);
    }
}