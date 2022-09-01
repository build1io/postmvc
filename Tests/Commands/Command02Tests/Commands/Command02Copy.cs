using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands
{
    public sealed class Command02Copy : Command<int, string>
    {
        public static Action<int, string> OnExecute;
        
        public override void Execute(int param01, string param02)
        {
            OnExecute?.Invoke(param01, param02);
        }
    }
}