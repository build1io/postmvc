using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00Retain : Command
    {
        public static Command00Retain Instance  { get; private set; }
        public static Exception       Exception { get; } = new("Test exception");

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
            Fail(Exception);
        }
    }
}