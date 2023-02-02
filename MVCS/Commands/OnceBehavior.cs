using System;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    [Flags]
    public enum OnceBehavior
    {
        Default = UnbindOnComplete | UnbindOnBreak | UnbindOnFail | UnbindOnTriggerFail,
        Anyway  = UnbindOnComplete | UnbindOnBreak | UnbindOnFail | UnbindOnTriggerFail,

        UnbindOnComplete    = 1 << 0,
        UnbindOnBreak       = 1 << 1,
        UnbindOnFail        = 1 << 2,
        UnbindOnTriggerFail = 1 << 3
    }
}