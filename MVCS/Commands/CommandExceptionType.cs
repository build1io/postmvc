namespace Build1.PostMVC.Core.MVCS.Commands
{
    public enum CommandExceptionType
    {
        AttemptToReleaseResolvedCommand = 1,
        AttemptToFailResolvedCommand    = 2,
        AttemptToRetainResolvedCommand  = 3
    }
}