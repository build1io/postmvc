using System;
using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.Unity.Mediation;

namespace Build1.PostMVC.Extensions.Unity.Events.Impl
{
    internal sealed class EventMapInfoUnityViewDispatcher : IEventMapInfo
    {
        private UnityViewDispatcher   _dispatcher;
        private Event                 _event;
        private Action                _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(UnityViewDispatcher dispatcher, Event @event, Action listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }

        public void OnceListener()
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Bind()
        {
            if (_isOnceScenario)
            {
                _dispatcher.AddListenerOnce(_event, OnceListener);
                _dispatcher.AddListenerOnce(_event, _listener);
                return;
            }

            _dispatcher.AddListener(_event, _listener);
        }

        public IEventMapInfo Unbind()
        {
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);

            _dispatcher.RemoveListener(_event, _listener);

            return this;
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfoUnityViewDispatcher<T1> : IEventMapInfo
    {
        private UnityViewDispatcher   _dispatcher;
        private Event<T1>             _event;
        private Action<T1>            _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(UnityViewDispatcher dispatcher, Event<T1> @event, Action<T1> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }

        public void OnceListener(T1 param01)
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Bind()
        {
            if (_isOnceScenario)
            {
                _dispatcher.AddListenerOnce(_event, OnceListener);
                _dispatcher.AddListenerOnce(_event, _listener);
                return;
            }

            _dispatcher.AddListener(_event, _listener);
        }

        public IEventMapInfo Unbind()
        {
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);

            _dispatcher.RemoveListener(_event, _listener);

            return this;
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfoUnityViewDispatcher<T1, T2> : IEventMapInfo
    {
        private UnityViewDispatcher   _dispatcher;
        private Event<T1, T2>         _event;
        private Action<T1, T2>        _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(UnityViewDispatcher dispatcher, Event<T1, T2> @event, Action<T1, T2> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }

        public void OnceListener(T1 param01, T2 param02)
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Bind()
        {
            if (_isOnceScenario)
            {
                _dispatcher.AddListenerOnce(_event, OnceListener);
                _dispatcher.AddListenerOnce(_event, _listener);
                return;
            }

            _dispatcher.AddListener(_event, _listener);
        }

        public IEventMapInfo Unbind()
        {
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);

            _dispatcher.RemoveListener(_event, _listener);

            return this;
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }

    internal sealed class EventMapInfoUnityViewDispatcher<T1, T2, T3> : IEventMapInfo
    {
        private UnityViewDispatcher   _dispatcher;
        private Event<T1, T2, T3>     _event;
        private Action<T1, T2, T3>    _listener;
        private Action<IEventMapInfo> _infoRemovalMethod;
        private bool                  _isOnceScenario;

        public void Setup(UnityViewDispatcher dispatcher, Event<T1, T2, T3> @event, Action<T1, T2, T3> listener, Action<IEventMapInfo> infoRemovalMethod, bool isOnceScenario)
        {
            _dispatcher = dispatcher;
            _event = @event;
            _listener = listener;
            _infoRemovalMethod = infoRemovalMethod;
            _isOnceScenario = isOnceScenario;
        }

        public void OnceListener(T1 param01, T2 param02, T3 param03)
        {
            _infoRemovalMethod.Invoke(this);
        }

        public void Bind()
        {
            if (_isOnceScenario)
            {
                _dispatcher.AddListenerOnce(_event, OnceListener);
                _dispatcher.AddListenerOnce(_event, _listener);
                return;
            }

            _dispatcher.AddListener(_event, _listener);
        }

        public IEventMapInfo Unbind()
        {
            if (_isOnceScenario)
                _dispatcher.RemoveListener(_event, OnceListener);

            _dispatcher.RemoveListener(_event, _listener);

            return this;
        }

        public bool Match(IEventDispatcher dispatcher, EventBase @event, object listener)
        {
            return ReferenceEquals(_dispatcher, dispatcher) &&
                   ReferenceEquals(_event, @event) &&
                   Equals(_listener, listener);
        }
    }
}