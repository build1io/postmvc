using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;

namespace Build1.PostMVC.Extensions.MVCS.Commands.Impl
{
    internal sealed class CommandParams : CommandParamsBase
    {
        public override bool TryExecuteCommand(CommandBase commandBase)
        {
            switch (commandBase)
            {
                case Command command00:
                    command00.Execute();
                    return true;
                
                default:
                    return false;
            }
        }
        
        public override void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
        {
            if (@event is Event event0)
                dispatcher.Dispatch(event0);
            else
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
        }
    }
    
    internal sealed class CommandParams<T1> : CommandParamsBase
    {
        public T1 Param01 { get; internal set; }

        public override bool TryExecuteCommand(CommandBase commandBase)
        {
            switch (commandBase)
            {
                case Command<T1> command01:
                    command01.Param01 = Param01;
                    command01.Execute(Param01);
                    return true;
                
                case Command command00:
                    command00.Execute();
                    return true;
                
                default:
                    return false;
            }
        }
        
        public override void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
        {
            switch (@event)
            {
                case Event<T1> event1:
                    dispatcher.Dispatch(event1, Param01);
                    break;
                case Event event0:
                    dispatcher.Dispatch(event0);
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
            }
        }
    }
    
    internal sealed class CommandParams<T1, T2> : CommandParamsBase
    {
        public T1 Param01 { get; internal set; }
        public T2 Param02 { get; internal set; }
        
        public override bool TryExecuteCommand(CommandBase commandBase)
        {
            switch (commandBase)
            {
                case Command<T1, T2> command02:
                    command02.Param01 = Param01;
                    command02.Param02 = Param02;
                    command02.Execute(Param01, Param02);
                    return true;
                
                case Command<T1> command01:
                    command01.Param01 = Param01;
                    command01.Execute(Param01);
                    return true;
                
                case Command command00:
                    command00.Execute();
                    return true;
                
                default:
                    return false;
            }
        }
        
        public override void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
        {
            switch (@event)
            {
                case Event<T1, T2> event2:
                    dispatcher.Dispatch(event2, Param01, Param02);
                    break;
                case Event<T1> event1:
                    dispatcher.Dispatch(event1, Param01);
                    break;
                case Event event0:
                    dispatcher.Dispatch(event0);
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
            }
        }
    }
    
    internal sealed class CommandParams<T1, T2, T3> : CommandParamsBase
    {
        public T1 Param01 { get; internal set; }
        public T2 Param02 { get; internal set; }
        public T3 Param03 { get; internal set; }
        
        public override bool TryExecuteCommand(CommandBase commandBase)
        {
            switch (commandBase)
            {
                case Command<T1, T2, T3> command03:
                    command03.Param01 = Param01;
                    command03.Param02 = Param02;
                    command03.Param03 = Param03;
                    command03.Execute(Param01, Param02, Param03);
                    return true;
                
                case Command<T1, T2> command02:
                    command02.Param01 = Param01;
                    command02.Param02 = Param02;
                    command02.Execute(Param01, Param02);
                    return true;
                
                case Command<T1> command01:
                    command01.Param01 = Param01;
                    command01.Execute(Param01);
                    return true;
                
                case Command command00:
                    command00.Execute();
                    return true;
                
                default:
                    return false;
            }
        }
        
        public override void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
        {
            switch (@event)
            {
                case Event<T1, T2, T3> event3:
                    dispatcher.Dispatch(event3, Param01, Param02, Param03);
                    break;
                case Event<T1, T2> event2:
                    dispatcher.Dispatch(event2, Param01, Param02);
                    break;
                case Event<T1> event1:
                    dispatcher.Dispatch(event1, Param01);
                    break;
                case Event event0:
                    dispatcher.Dispatch(event0);
                    break;
                default:
                    throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
            }
        }
    }
}