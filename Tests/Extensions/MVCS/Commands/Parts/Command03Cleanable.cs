using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command03Cleanable : Command<int, string, CommandData>
    {
        public static Action<int, string, CommandData> OnExecute;
        public static Action                           OnCleaning;
        
        public Command03Cleanable()
        {
            ClearOnRelease = true;
        }
        
        public override void Execute(int param01, string param02, CommandData param03)
        {
            OnExecute?.Invoke(param01, param02, param03);
        }
        
        protected override void OnClear()
        {
            OnCleaning?.Invoke();
        }
    }
}