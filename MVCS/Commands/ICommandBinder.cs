using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands
{
    public interface ICommandBinder
    {
        CommandBinding             Bind(Event @event);
        CommandBinding<T1>         Bind<T1>(Event<T1> @event);
        CommandBinding<T1, T2>     Bind<T1, T2>(Event<T1, T2> @event);
        CommandBinding<T1, T2, T3> Bind<T1, T2, T3>(Event<T1, T2, T3> @event);

        void Unbind(ICommandBinding binding);
        void UnbindAll(EventBase @event);

        FlowBinding             Flow();
        FlowBinding<T1>         Flow<T1>();
        FlowBinding<T1, T2>     Flow<T1, T2>();
        FlowBinding<T1, T2, T3> Flow<T1, T2, T3>();

        SingleCommandBinding             Command<TCommand>() where TCommand : Command;
        SingleCommandBinding<T1>         Command<TCommand, T1>() where TCommand : Command<T1>;
        SingleCommandBinding<T1, T2>     Command<TCommand, T1, T2>() where TCommand : Command<T1, T2>;
        SingleCommandBinding<T1, T2, T3> Command<TCommand, T1, T2, T3>() where TCommand : Command<T1, T2, T3>;

        public IList<CommandBinding>             GetBindings(Event @event);
        public IList<CommandBinding<T1>>         GetBindings<T1>(Event<T1> @event);
        public IList<CommandBinding<T1, T2>>     GetBindings<T1, T2>(Event<T1, T2> @event);
        public IList<CommandBinding<T1, T2, T3>> GetBindings<T1, T2, T3>(Event<T1, T2, T3> @event);

        void ProcessEvent(Event @event);
        void ProcessEvent<T1>(Event<T1> @event, T1 param01);
        void ProcessEvent<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02);
        void ProcessEvent<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03);

        void Break(ICommandBinding binding);
        void BreakAll(EventBase @event);
    }
}