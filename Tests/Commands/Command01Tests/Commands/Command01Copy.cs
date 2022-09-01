using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01Copy : Command<int>
    {
        public static Action<int> OnExecute;
        
        public override void Execute(int param01)
        {
            OnExecute?.Invoke(param01);
        }
    }
}