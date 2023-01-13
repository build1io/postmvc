using Build1.PostMVC.Core.MVCS.Events;
using Build1.PostMVC.Core.MVCS.Events.Impl;

namespace Build1.PostMVC.Core.MVCS.Commands.Impl
{
    internal interface ICommandParams
    {
        void DispatchParams(IEventDispatcher dispatcher, EventBase @event);
    }
    
    internal interface ICommandParams<out T1> : ICommandParams
    {
        T1 Param01 { get; }
    }
    
    internal interface ICommandParams<out T1, out T2> : ICommandParams<T1>
    {
        T2 Param02 { get; }
    }
    
    internal interface ICommandParams<out T1, out T2, out T3> : ICommandParams<T1, T2>
    {
        T3 Param03 { get; }
    }
}