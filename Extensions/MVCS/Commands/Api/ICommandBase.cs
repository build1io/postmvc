namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommandBase
    {
        int  Index      { get; }
        bool IsClean    { get; }
        bool IsRetained { get; }
        bool IsFailed   { get; }

        void SetCommandBinder(ICommandBinder commandBinder);
        void Reset();
    }
}