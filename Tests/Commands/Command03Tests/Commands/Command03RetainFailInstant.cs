using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;

namespace Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands
{
    public sealed class Command03RetainFailInstant : Command<int, string, CommandData>
    {
        public static Command03RetainFailInstant Instance { get; private set; }

        public static Action<int, string, CommandData> OnExecute;

        public override void Execute(int param01, string param02, CommandData param03)
        {
            Instance = this;
            OnExecute?.Invoke(param01, param02, param03);

            Retain();
            Fail(new Exception("Test exception"));
        }
    }
}