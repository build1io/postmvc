using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command02Exception : Command<int, string>
    {
        public static event Action<int, string> OnExecute;
        
        public override void Execute(int param01, string param02)
        {
            OnExecute?.Invoke(param01, param02);
            throw new Exception("Test exception");        
        }
    }
}