using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00 : Command
    {
        public static Action OnExecute;
        
        public override void Execute()
        {
            OnExecute?.Invoke();
        }
    }
}