using System;
using Build1.PostMVC.Core.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
{
    public sealed class Command00DoubleFail : Command
    {
        public override void Execute()
        {
            Fail(new Exception("Test exception"));
            Fail(new Exception("Test exception"));
        }
    }
}