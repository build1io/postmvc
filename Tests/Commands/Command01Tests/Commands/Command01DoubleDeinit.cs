using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01DoubleDeinit : Command<int>
    {
        public static Action      OnPostConstruct;
        public static Action<int> OnExecute;
        public static Action      OnPreDestroy;

        [PostConstruct]
        public void PostConstruct()
        {
            OnPostConstruct?.Invoke();
        }

        [PreDestroy]
        public void PreDestroy()
        {
            OnPreDestroy?.Invoke();
        }

        public override void Execute(int param01)
        {
            OnExecute?.Invoke(param01);
            
            Retain();
            Release();
        }
    }
}