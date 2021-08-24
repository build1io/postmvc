namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommand : ICommandBase
    {
        void Setup(int sequenceId);
        void Execute();
    }
    
    public interface ICommand<T1> : ICommandBase
    {
        T1 Param01 { get; }

        void Setup(int sequenceId, T1 param01);
        void Execute(T1 param01);
    }
    
    public interface ICommand<T1, T2> : ICommandBase
    {
        T1 Param01 { get; }
        T2 Param02 { get; }
        
        void Setup(int sequenceId, T1 param01, T2 param02);
        void Execute(T1 param01, T2 param02);
    }
    
    public interface ICommand<T1, T2, T3> : ICommandBase
    {
        T1 Param01 { get; }
        T2 Param02 { get; }
        T3 Param03 { get; }
        
        void Setup(int sequenceId, T1 param01, T2 param02, T3 param03);
        void Execute(T1 param01, T2 param02, T3 param03);
    }
}