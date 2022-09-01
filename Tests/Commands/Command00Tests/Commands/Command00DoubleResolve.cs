using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command00Tests.Commands
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