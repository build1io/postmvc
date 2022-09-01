using Build1.PostMVC.Core.MVCS.Commands;

namespace Build1.PostMVC.Core.Tests.Commands.Command02Tests.Commands
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