using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands
{
    public sealed class Command03Exception : Command<int, string, CommandData>
    {
        public static Action<int, string, CommandData> OnExecute;
        
        public override void Execute(int param01, string param02, CommandData param03)
        {
            OnExecute?.Invoke(param01, param02, param03);
            throw new Exception("Test exception");
        }
    }
}