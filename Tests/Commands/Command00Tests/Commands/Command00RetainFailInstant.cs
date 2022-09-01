using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00RetainFailInstant : Command
    {
        public static Command00RetainFailInstant Instance { get; private set; }

        public static Action OnExecute;

        public override void Execute()
        {
            Instance = this;
            OnExecute?.Invoke();

            Retain();
            Fail(new Exception("Test exception"));
        }
    }
}