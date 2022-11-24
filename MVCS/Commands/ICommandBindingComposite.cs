using System;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    internal interface ICommandBindingComposite
    {
        void ForEachBinding(Action<CommandBindingBase> handler);
    }
}