using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command02Retaining : Command<int, string>
    {
        public static Command02Retaining Instance { get; private set; }
        
        public static Action<int, string> OnExecute;
        
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
    }
}