using System;
using Build1.PostMVC.Core.MVCS.Commands;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00DoubleDeinit : Command
    {
        public static Action OnPostConstruct;
        public static Action OnExecute;
        public static Action OnPreDestroy;

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
        
        public override void Execute()
        {
            OnExecute?.Invoke();
            
            Retain();
            Release();
        }
    }
}