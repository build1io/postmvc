using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command03Retaining : Command<int, string, CommandData>
    {
        public static Command03Retaining Instance { get; private set; }
        
        public static Action<int, string, CommandData> OnExecute;
        
        public override void Execute(int param01, string param02, CommandData param03)
        {
            Instance = this;
            OnExecute?.Invoke(param01, param02, param03);
            Retain();
        }
        
        public void ReleaseImpl()
        {
            Release();
        }
    }
}