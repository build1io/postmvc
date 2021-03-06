using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Commands.Api;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands
{
    public interface ICommandBinder
    {
        CommandBinding             Bind(Event type);
        CommandBinding<T1>         Bind<T1>(Event<T1> type);
        CommandBinding<T1, T2>     Bind<T1, T2>(Event<T1, T2> type);
        CommandBinding<T1, T2, T3> Bind<T1, T2, T3>(Event<T1, T2, T3> type);

        void Unbind(CommandBindingBase binding);
        void UnbindAll(EventBase type);
        void UnbindAll();
        void UnbindOnQuit();

        IList<CommandBindingBase> GetBindings(EventBase type);

        void ReleaseCommand(ICommand command);
        void ReleaseCommand<T1>(ICommand<T1> command);
        void ReleaseCommand<T1, T2>(ICommand<T1, T2> command);
        void ReleaseCommand<T1, T2, T3>(ICommand<T1, T2, T3> command);

        void StopCommand(ICommandBase command);

        void ProcessEvent(Event type);
        void ProcessEvent<T1>(Event<T1> type, T1 param01);
        void ProcessEvent<T1, T2>(Event<T1, T2> type, T1 param01, T2 param02);
        void ProcessEvent<T1, T2, T3>(Event<T1, T2, T3> type, T1 param01, T2 param02, T3 param03);
    }
}