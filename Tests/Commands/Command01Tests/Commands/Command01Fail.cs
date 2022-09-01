using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01Fail : Command<int>
    {
        public static Action<int> OnExecute;
        
        public override void Execute(int param01)
        {
            OnExecute?.Invoke(param01);
            Fail(new Exception("Test exception"));
        }
    }
}