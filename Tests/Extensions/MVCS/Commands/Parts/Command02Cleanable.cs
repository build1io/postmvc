using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command02Cleanable : Command<int, string>
    {
        public static Action<int, string> OnExecute;
        public static Action              OnCleaning;
        
        public Command02Cleanable()
        {
            ClearOnRelease = true;
        }
        
        public override void Execute(int param01, string param02)
        {
            OnExecute?.Invoke(param01, param02);
        }
        
        protected override void OnClear()
        {
            OnCleaning?.Invoke();
        }
    }
}