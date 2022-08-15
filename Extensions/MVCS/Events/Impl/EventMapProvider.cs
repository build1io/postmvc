using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    public sealed class EventMapProvider : InjectionProvider<IEventMap>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }
        [Inject] public IEventBus        EventBus   { get; set; }

        private readonly Stack<IEventMap> _availableInstances;
        private readonly List<IEventMap>  _usedInstances;
        private readonly EventMapInfoPool _infoPools;

        public EventMapProvider()
        {
            _availableInstances = new Stack<IEventMap>();
            _usedInstances = new List<IEventMap>();
            _infoPools = new EventMapInfoPool();
        }

        /*
         * Public.
         */

        public override IEventMap TakeInstance(object parent, Inject attribute)
        {
            IEventMap map;

            if (_availableInstances.Count > 0)
            {
                map = _availableInstances.Pop();
                _usedInstances.Add(map);
            }
            else
            {
                map = CreateEventMapper();
                _usedInstances.Add(map);
            }

            return map;
        }

        public override void ReturnInstance(IEventMap instance)
        {
            if (!_usedInstances.Remove(instance))
                return;

            instance.UnmapAll();
            _availableInstances.Push(instance);
        }

        /*
         * Protected.
         */

        private IEventMap CreateEventMapper()
        {
            return new EventMap(Dispatcher, EventBus, _infoPools);
        }
    }
}