using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command02DoubleDeinit : Command<int, string>
    {
        public static event Action              OnPostConstruct;
        public static event Action<int, string> OnExecute;
        public static event Action              OnPreDestroy;

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