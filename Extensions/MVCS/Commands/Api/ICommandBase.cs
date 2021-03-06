namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommandBase
    {
        int  SequenceId     { get; }
        bool IsRetained     { get; }
        bool IsClean        { get; }
        bool ClearOnRelease { get; }

        void SetCommandBinder(ICommandBinder commandBinder);
        void SetSequenceId(int sequenceId);

        void Clear();
    }
}