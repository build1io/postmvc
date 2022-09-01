using System;
using Build1.PostMVC.Core.Extensions.MVCS.Events;

namespace Build1.PostMVC.Core.Extensions.MVCS.Commands
{
    public interface ICommandBindingBase
    {
        ICommandBindingBase OnFail(Event<Exception> @event);
        ICommandBindingBase OnFail(Event @event);

        ICommandBindingBase InParallel();
        ICommandBindingBase InSequence();
        ICommandBindingBase Once();
        ICommandBindingBase Once(OnceBehavior behavior);
        ICommandBindingBase UnbindOnQuit();
    }
}