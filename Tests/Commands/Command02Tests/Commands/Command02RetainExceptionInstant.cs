using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands
{
    public sealed class Command02RetainExceptionInstant : Command<int, string>
    {
        public static Command02RetainExceptionInstant Instance { get; private set; }

        public static Action<int, string> OnExecute;

        public override void Execute(int param01, string param02)
        {
            Instance = this;
            OnExecute?.Invoke(param01, param02);

            Retain();
            throw new Exception("Test exception");
        }
    }
}