using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command01Retaining : Command<int>
    {
        public static Command01Retaining Instance { get; private set; }

        public static Action<int> OnExecute;
        
        public override void Execute(int param01)
        {
            Instance = this;
            OnExecute?.Invoke(param01);
            Retain();
        }

        public void ReleaseImpl()
        {
            Release();
        }
    }
}