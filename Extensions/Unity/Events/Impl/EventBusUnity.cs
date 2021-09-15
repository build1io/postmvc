using Build1.PostMVC.Extensions.MVCS.Events;
using Build1.PostMVC.Extensions.MVCS.Events.Impl;
using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.Unity.Modules.Update;
using Event = Build1.PostMVC.Extensions.MVCS.Events.Event;

namespace Build1.PostMVC.Extensions.Unity.Events.Impl
{
    public sealed class EventBusUnity : IEventBus
    {
        [Inject] public IInjectionBinder  InjectionBinder  { get; set; }
        [Inject] public IUpdateController UpdateController { get; set; }

        public bool HasEvents => _bus.HasEvents;

        private IEventBus _bus;
        private bool      _subscribed;

        [PostConstruct]
        public void PostConstruct()
        {
            _bus = InjectionBinder.Construct<EventBus>(true);
        }

        [PreDestroy]
        public void PreDestroy()
        {
            InjectionBinder.Destroy(_bus, true);
            _bus = null;
        }

        /*
         * Public.
         */

        public void Add(Event @event)
        {
            _bus.Add(@event);
            Subscribe();
        }

        public void Add<T1>(Event<T1> @event, T1 param01)
        {
            _bus.Add(@event, param01);
            Subscribe();
        }

        public void Add<T1, T2>(Event<T1, T2> @event, T1 param01, T2 param02)
        {
            _bus.Add(@event, param01, param02);
            Subscribe();
        }

        public void Add<T1, T2, T3>(Event<T1, T2, T3> @event, T1 param01, T2 param02, T3 param03)
        {
            _bus.Add(@event, param01, param02, param03);
            Subscribe();
        }

        public void Dispatch()
        {
            _bus.Dispatch();
        }

        /*
         * Private.
         */

        private void Subscribe()
        {
            if (_subscribed)
                return;

            _subscribed = true;
            UpdateController.SubscribeForUpdate(OnUpdate);
        }

        private void Unsubscribe()
        {
            if (!_subscribed)
                return;

            _subscribed = false;
            UpdateController.Unsubscribe(OnUpdate);
        }

        private void OnUpdate(float deltaTime)
        {
            _bus.Dispatch();

            if (!_bus.HasEvents)
                Unsubscribe();
        }
    }
}