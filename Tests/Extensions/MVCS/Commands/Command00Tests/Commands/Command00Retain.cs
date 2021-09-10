using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands
{
    public sealed class Command00Retain : Command
    {
        public static Command00Retain Instance { get; private set; }

        public static Action OnExecute;
        public static Action OnFail;
        
        public override void Execute()
        {
            Instance = this;
            OnExecute?.Invoke();
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