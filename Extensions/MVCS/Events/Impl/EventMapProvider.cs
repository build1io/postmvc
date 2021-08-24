using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal sealed class EventMapProvider : InjectionProvider<Inject, IEventMap>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        private readonly Stack<IEventMap> _availableInstances;
        private readonly List<IEventMap>  _usedInstances;

        public EventMapProvider()
        {
            _availableInstances = new Stack<IEventMap>();
            _usedInstances = new List<IEventMap>();
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
                // Specifying EventDispatcherWithCommandProcessing is bad but needed to escape AOT issues.
                map = CreateEventMap(); 
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

        private IEventMap CreateEventMap()
        {
            return new EventMap((EventDispatcherWithCommandProcessing)Dispatcher);
        }
    }
}