namespace Build1.PostMVC.Core.Extensions.MVCS.Commands
{
    public enum CommandBinderExceptionType
    {
        BindingAlreadyExecuting = 1,
        IncompatibleEventType   = 2,
        IncompatibleCommand     = 3
    }
}