using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands
{
    public sealed class Command02RetainCopy : Command<int, string>
    {
        public static Command02RetainCopy Instance { get; private set; }
        
        public static Action<int, string> OnExecute;
        public static Action              OnFail;
        
        public override void Execute(int param01, string param02)
        {
            Instance = this;
            OnExecute?.Invoke(param01, param02);
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