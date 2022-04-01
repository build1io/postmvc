using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands
{
    public sealed class Command00Break : Command
    {
        public static Action OnExecute;
        
        public override void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}