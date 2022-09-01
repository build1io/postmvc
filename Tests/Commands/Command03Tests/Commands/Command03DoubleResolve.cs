using Build1.PostMVC.Core.Extensions.MVCS.Commands;
using Build1.PostMVC.Core.Tests.Commands.Common;

namespace Build1.PostMVC.Core.Tests.Commands.Command03Tests.Commands
{
    public sealed class Command03DoubleResolve : Command<int, string, CommandData>
    {
        public override void Execute(int param01, string param02, CommandData param03)
        {
            Release();
            Release();
        }
    }
}