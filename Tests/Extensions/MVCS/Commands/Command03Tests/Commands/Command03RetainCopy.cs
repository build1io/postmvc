using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands
{
    public sealed class Command03RetainCopy : Command<int, string, CommandData>
    {
        public static Command03RetainCopy Instance { get; private set; }

        public static Action<int, string, CommandData> OnExecute;
        public static Action                           OnFail;

        public override void Execute(int param01, string param02, CommandData param03)
        {
            Instance = this;
            OnExecute?.Invoke(param01, param02, param03);
            Retain();
        }

        public void ReleaseImpl()
        {
            Release();
        }

        public void FailImpl()
        {
            OnFail?.Invoke();
            Fail(new Exception("Test exception"));
        }
    }
}