using System;
using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands.Impl
{
    internal class CommandParamsByGetter : ICommandParams
    {
        public void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
        {
            if (@event is Event event0)
                dispatcher.Dispatch(event0);
            else
                throw new CommandBinderException(CommandBinderExceptionType.IncompatibleEventType);
        }
    }
    
    internal class CommandParamsByGetter<T1> : ICommandParams<T1>
    {
        public T1 Param01 => _param01Func.Invoke();

        private Func<T1> _param01Func;
        
        internal void SetParam01Func(Func<T1> param01Func) { _param01Func = param01Func; }
        
        public void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
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
    
    internal class CommandParamsByGetter<T1, T2> : ICommandParams<T1, T2>
    {
        public T1 Param01 => _param01Func.Invoke();
        public T2 Param02 => _param02Func.Invoke();

        private Func<T1> _param01Func;
        private Func<T2> _param02Func;
        
        internal void SetParam01Func(Func<T1> param01Func) { _param01Func = param01Func; }
        internal void SetParam02Func(Func<T2> param02Func) { _param02Func = param02Func; }

        public void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
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
    
    internal class CommandParamsByGetter<T1, T2, T3> : ICommandParams<T1, T2>
    {
        public T1 Param01 => _param01Func.Invoke();
        public T2 Param02 => _param02Func.Invoke();
        public T3 Param03 => _param03Func.Invoke();

        private Func<T1> _param01Func;
        private Func<T2> _param02Func;
        private Func<T3> _param03Func;
        
        internal void SetParam01Func(Func<T1> param01Func) { _param01Func = param01Func; }
        internal void SetParam02Func(Func<T2> param02Func) { _param02Func = param02Func; }
        internal void SetParam03Func(Func<T3> param03Func) { _param03Func = param03Func; }
        
        public void DispatchParams(IEventDispatcher dispatcher, EventBase @event)
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