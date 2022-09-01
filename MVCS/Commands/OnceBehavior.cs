using System;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    [Flags]
    public enum OnceBehavior
    {
        None = 0,

        Default = UnbindOnComplete | UnbindOnBreak | UnbindOnFail,
        Anyway  = UnbindOnComplete | UnbindOnBreak | UnbindOnFail,

        UnbindOnComplete = 1 << 0,
        UnbindOnBreak    = 1 << 1,
        UnbindOnFail     = 1 << 2
    }
}