using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01RetainReleaseInstant : Command<int>
    {
        public static Command01RetainReleaseInstant Instance { get; private set; }

        public static Action<int> OnExecute;

        public override void Execute(int param01)
        {
            Instance = this;
            OnExecute?.Invoke(param01);

            Retain();
            Release();
        }
    }
}