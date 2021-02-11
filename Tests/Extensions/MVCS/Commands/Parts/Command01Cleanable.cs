using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command01Cleanable : Command<int>
    {
        public static Action<int> OnExecute;
        public static Action      OnCleaning;

        public Command01Cleanable()
        {
            ClearOnRelease = true;
        }

        public override void Execute(int param01)
        {
            OnExecute?.Invoke(param01);
        }

        protected override void OnClear()
        {
            OnCleaning?.Invoke();
        }
    }
}