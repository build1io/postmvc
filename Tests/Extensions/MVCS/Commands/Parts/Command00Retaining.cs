using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command00Retaining : Command
    {
        public static Command00Retaining Instance { get; private set; }

        public static Action OnExecute;
        
        public override void Execute()
        {
            Instance = this;
            OnExecute?.Invoke();
            Retain();
        }

        public void ReleaseImpl()
        {
            Release();
        }
    }
}