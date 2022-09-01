using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;

namespace Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands
{
    public sealed class Command03Retain : Command<int, string, CommandData>
    {
        public static Command03Retain Instance { get; private set; }

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
        
        public void BreakImpl()
        {
            Break();
        }

        public void FailImpl()
        {
            OnFail?.Invoke();
            Fail(new Exception("Test exception"));
        }
    }
}