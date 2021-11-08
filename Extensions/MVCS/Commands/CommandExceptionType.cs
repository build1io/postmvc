namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public enum CommandExceptionType
    {
        AttemptToReleaseResolvedCommand = 1,
        AttemptToFailResolvedCommand    = 2,
        AttemptToRetainResolvedCommand  = 3
    }
}