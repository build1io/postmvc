using System;
using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands
{
    public sealed class Command02DoubleFail : Command<int, string>
    {
        public override void Execute(int param01, string param02)
        {
            Fail(new Exception("Test exception"));
            Fail(new Exception("Test exception"));
        }
    }
}