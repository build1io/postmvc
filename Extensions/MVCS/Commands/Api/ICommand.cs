namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommand : ICommandBase
    {
        CommandBinding Binding { get; }

        void PreExecute(CommandBinding binding);
        void Execute();
    }

    public interface ICommand<T1> : ICommandBase
    {
        CommandBinding<T1> Binding { get; }

        T1 Param01 { get; }

        void PreExecute(CommandBinding<T1> binding, T1 param01);
        void Execute(T1 param01);
    }

    public interface ICommand<T1, T2> : ICommandBase
    {
        CommandBinding<T1, T2> Binding { get; }

        T1 Param01 { get; }
        T2 Param02 { get; }

        void PreExecute(CommandBinding<T1, T2> binding, T1 param01, T2 param02);
        void Execute(T1 param01, T2 param02);
    }

    public interface ICommand<T1, T2, T3> : ICommandBase
    {
        CommandBinding<T1, T2, T3> Binding { get; }

        T1 Param01 { get; }
        T2 Param02 { get; }
        T3 Param03 { get; }

        void PreExecute(CommandBinding<T1, T2, T3> binding, T1 param01, T2 param02, T3 param03);
        void Execute(T1 param01, T2 param02, T3 param03);
    }
}