using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands
{
    public sealed class Command02DoubleDeinit : Command<int, string>
    {
        public static Action              OnPostConstruct;
        public static Action<int, string> OnExecute;
        public static Action              OnPreDestroy;

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

        public override void Execute(int param01, string param02)
        {
            OnExecute?.Invoke(param01, param02);

            Retain();
            Release();
        }
    }
}