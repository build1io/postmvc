using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Extensions.MVCS.Events.Impl
{
    internal class EventMapProvider : InjectionProvider<Inject, IEventMapper>
    {
        [Inject] public IEventDispatcher Dispatcher { get; set; }

        private readonly Stack<IEventMapper> _availableInstances;
        private readonly List<IEventMapper>  _usedInstances;

        public EventMapProvider()
        {
            _availableInstances = new Stack<IEventMapper>();
            _usedInstances = new List<IEventMapper>();
        }

        /*
         * Public.
         */

        public override IEventMapper TakeInstance(object parent, Inject attribute)
        {
            IEventMapper mapper;
            
            if (_availableInstances.Count > 0)
            {
                mapper = _availableInstances.Pop();
                _usedInstances.Add(mapper);
            }
            else
            {
                // Specifying EventDispatcherWithCommandProcessing is bad but needed to escape AOT issues.
                mapper = CreateEventMapper(); 
                _usedInstances.Add(mapper);
            }

            return mapper;
        }

        public override void ReturnInstance(IEventMapper instance)
        {
            if (!_usedInstances.Remove(instance))
                return;
            
            instance.UnmapAll();
            _availableInstances.Push(instance);
        }
        
        /*
         * Protected.
         */

        protected virtual IEventMapper CreateEventMapper()
        {
            return new EventMapper((EventDispatcherWithCommandProcessing)Dispatcher);
        }
    }
}