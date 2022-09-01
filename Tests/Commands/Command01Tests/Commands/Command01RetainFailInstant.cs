using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01RetainFailInstant : Command<int>
    {
        public static Command01RetainFailInstant Instance { get; private set; }

        public static Action<int> OnExecute;

        public override void Execute(int param01)
        {
            Instance = this;
            OnExecute?.Invoke(param01);

            Retain();
            Fail(new Exception("Test exception"));
        }
    }
}