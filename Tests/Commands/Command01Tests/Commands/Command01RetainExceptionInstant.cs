using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01RetainExceptionInstant : Command<int>
    {
        public static Command01RetainExceptionInstant Instance { get; private set; }

        public static Action<int> OnExecute;

        public override void Execute(int param01)
        {
            Instance = this;
            OnExecute?.Invoke(param01);

            Retain();
            throw new Exception("Test exception");
        }
    }
}