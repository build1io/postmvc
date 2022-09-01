using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00RetainReleaseInstant : Command
    {
        public static Command00RetainReleaseInstant Instance { get; private set; }

        public static Action OnExecute;

        public override void Execute()
        {
            Instance = this;
            OnExecute?.Invoke();

            Retain();
            Release();
        }
    }
}