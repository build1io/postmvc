using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command00Cleanable : Command
    {
        public static Action OnExecute;
        public static Action OnCleaning;
        
        public Command00Cleanable()
        {
            ClearOnRelease = true;
        }

        public override void Execute()
        {
            OnExecute?.Invoke();
        }

        protected override void OnClear()
        {
            OnCleaning?.Invoke();
        }
    }
}