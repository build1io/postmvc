using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class CommandCleanable : Command
    {
        public Action OnClearing;
        
        public CommandCleanable()
        {
            ClearOnRelease = true;
        }

        public override void Execute()
        {
        }

        protected override void OnClear()
        {
            OnClearing?.Invoke();
        }
    }
}