using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public class CommandCleanableNot : Command
    {
        public Action OnClearing;
        
        public CommandCleanableNot()
        {
            ClearOnRelease = false;
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