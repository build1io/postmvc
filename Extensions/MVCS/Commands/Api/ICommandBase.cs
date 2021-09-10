using System;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Api
{
    public interface ICommandBase
    {
        int       Index      { get; }
        Exception Exception  { get; }
        bool      IsExecuted { get; }
        bool      IsRetained { get; }
        bool      IsFailed   { get; }
        bool      IsClean    { get; }

        void SetCommandBinder(ICommandBinder commandBinder);
        void Setup(int index);
        void SetExecuted();
        void Reset();
    }
}