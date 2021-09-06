using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Parts
{
    public sealed class Command03DoubleDeinit : Command<int, string, CommandData>
    {
        public static event Action                           OnPostConstruct;
        public static event Action<int, string, CommandData> OnExecute;
        public static event Action                           OnPreDestroy;

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

        public override void Execute(int param01, string param02, CommandData param03)
        {
            OnExecute?.Invoke(param01, param02, param03);

            Retain();
            Release();
        }
    }
}