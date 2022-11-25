namespace Build1.PostMVC.Core.MVCS.Commands
{
    public enum CommandBinderExceptionType
    {
        BindingAlreadyExecuting = 1,
        FlowAlreadyExecuting    = 2,

        IncompatibleEventType = 10,
        IncompatibleCommand   = 11,

        UnknownBindingType = 20
    }
}