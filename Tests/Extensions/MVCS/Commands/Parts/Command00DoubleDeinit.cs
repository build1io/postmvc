using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command00DoubleDeinit : Command
    {
        public static event Action OnPostConstruct;
        public static event Action OnExecute;
        public static event Action OnPreDestroy;

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