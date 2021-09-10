using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands
{
    public sealed class Command02Retain : Command<int, string>
    {
        public static Command02Retain Instance { get; private set; }
        
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
        
        public void FailImpl()
        {
            OnFail?.Invoke();
            Fail(new Exception("Test exception"));
        }
    }
}