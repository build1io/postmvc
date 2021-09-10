using System;
using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common
{
    public sealed class CommandFailHandler : Command<Exception>
    {
        public static Action<Exception> OnExecute;
        
        public override void Execute(Exception exception)
        {
            OnExecute?.Invoke(exception);
        }
    }
}