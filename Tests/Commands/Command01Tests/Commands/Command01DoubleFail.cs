using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command01Tests.Commands
{
    public sealed class Command01DoubleFail : Command<int>
    {
        public override void Execute(int param01)
        {
            Fail(new Exception("Test exception"));
            Fail(new Exception("Test exception"));
        }
    }
}