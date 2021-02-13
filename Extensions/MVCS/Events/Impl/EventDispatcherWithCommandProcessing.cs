using System;
using Build1.PostMVC.Extensions.MVCS.Commands;
using Build1.PostMVC.Extensions.MVCS.Commands.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    public sealed class EventDispatcherWithCommandProcessing : IEventDispatcher
    {
        [Inject] public ICommandBinder CommandBinder
        {
            get => _commandBinder;
            set => _commandBinder = (CommandBinder)value;
        }  

        private readonly IEventDispatcher _dispatcher;
        private          CommandBinder    _commandBinder; // The end type must be specified to escape AOT issues.

        public EventDispatcherWithCommandProcessing()
        {
            _dispatcher = new EventDispatcher();
        }

        /*
         * Add.
         */

        public void AddListener(Event @event, Action listener)                                     { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListener(@event, listener); }
        public void AddListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListener(@event, listener); }

        /*
         * Add Once.
         */

        public void AddListenerOnce(Event @event, Action listener)                                     { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.AddListenerOnce(@event, listener); }
        public void AddListenerOnce<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.AddListenerOnce(@event, listener); }

        /*
         * Remove.
         */

        public void RemoveListener(Event @event, Action listener)                                     { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1>(Event<T1> @event, Action<T1> listener)                         { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2>(Event<T1, T2> @event, Action<T1, T2> listener)             { _dispatcher.RemoveListener(@event, listener); }
        public void RemoveListener<T1, T2, T3>(Event<T1, T2, T3> @event, Action<T1, T2, T3> listener) { _dispatcher.RemoveListener(@event, listener); }

        /*
         * Remove All.
         */

        public void RemoveAllListeners()                 { _dispatcher.RemoveAllListeners(); }
        public void RemoveAllListeners(EventBase @event) { _dispatcher.RemoveAllListeners(@event); }

        /*
         * Dispatch.
         */

        public void Dispatch(Event @event)
        {
            _dispatcher.Dispatch(@event);
            _commandBinder.ProcessEvent(@event);
        }

        public void Dispatch<T1>(Event<T1> @event, T1 param01)
        {
            _dispatcher.Dispatch(@event, param01);
            _commandBinder.ProcessEvent(@event, param01);
        }

        public void Dispatch<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)
        {
            _dispatcher.Dispatch(@event, param01, param02);
            _commandBinder.ProcessEvent(@event, param01, param02);
        }

        public void Dispatch<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            _dispatcher.Dispatch(@event, param01, param02, param03);
            _commandBinder.ProcessEvent(@event, param01, param02, param03);
        }
    }
}