using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command00Tests.Commands
{
    public sealed class Command00DoubleResolve : Command
    {
        public override void Execute()
        {
            Release();
            Release();
        }
    }
}