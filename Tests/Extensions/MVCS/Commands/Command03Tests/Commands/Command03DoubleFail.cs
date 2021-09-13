using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Tests.Extensions.MVCS.Commands.Common;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command03Tests.Commands
{
    public sealed class Command03DoubleFail : Command<int, string, CommandData>
    {
        public override void Execute(int param01, string param02, CommandData param03)
        {
            Fail(new Exception("Test exception"));
            Fail(new Exception("Test exception"));
        }
    }
}