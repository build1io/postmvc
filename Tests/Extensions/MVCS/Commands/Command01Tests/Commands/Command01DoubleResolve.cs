using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command01Tests.Commands
{
    public sealed class Command01DoubleResolve : Command<int>
    {
        public override void Execute(int param01)
        {
            Release();
            Release();
        }
    }
}