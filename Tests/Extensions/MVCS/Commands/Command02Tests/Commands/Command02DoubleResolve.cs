using Build1.PostMVC.Extensions.MVCS.Commands;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Commands.Command02Tests.Commands
{
    public sealed class Command02DoubleResolve : Command<int, string>
    {
        public override void Execute(int param01, string param02)
        {
            Release();
            Release();
        }
    }
}