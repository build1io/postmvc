using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands
{
    [Poolable]
    public sealed class Command00Exception : Command
    {
        public static Action OnExecute;
        
        public override void Execute()
        {
            OnExecute?.Invoke();
            throw new Exception("Test exception");
        }
    }
}