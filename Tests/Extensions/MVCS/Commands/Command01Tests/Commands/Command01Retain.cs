using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands
{
    public sealed class Command01Retain : Command<int>
    {
        public static Command01Retain Instance { get; private set; }

        public static Action<int> OnExecute;
        public static Action      OnFail;
        
        public override void Execute(int param01)
        {
            Instance = this;
            OnExecute?.Invoke(param01);
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