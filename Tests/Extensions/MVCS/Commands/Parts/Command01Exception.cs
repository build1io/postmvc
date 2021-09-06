using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command01Exception : Command<int>
    {
        public static event Action<int> OnExecute;
        
        public override void Execute(int param01)
        {
            OnExecute?.Invoke(param01);
            throw new Exception("Test exception");
        }
    }
}