using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00RetainCopy : Command
    {
        public static Command00RetainCopy Instance { get; private set; }

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