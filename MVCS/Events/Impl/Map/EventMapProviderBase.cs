using System.Collections.Generic;
using Build1.PostMVC.Core.MVCS.Injection;

namespace Build1.PostMVC.Core.MVCS.Events.Impl.Map
{
    public abstract class EventMapProviderBase<T> : InjectionProvider<T>
    {
        protected readonly EventMapInfoPool _infoPools;
        
        private readonly Stack<T> _availableInstances;
        private readonly List<T>  _usedInstances;

        protected EventMapProviderBase()
        {
            _infoPools = new EventMapInfoPool();
            
            _availableInstances = new Stack<T>();
            _usedInstances = new List<T>();
        }

        /*
         * Public.
         */

        public override T TakeInstance(object parent, Inject attribute)
        {
            T map;

            if (_availableInstances.Count > 0)
            {
                map = _availableInstances.Pop();
                _usedInstances.Add(map);
            }
            else
            {
                map = CreateMap();
                _usedInstances.Add(map);
            }

            return map;
        }

        public override void ReturnInstance(T instance)
        {
            if (!_usedInstances.Remove(instance))
                return;

            OnMapReturn(instance);

            _availableInstances.Push(instance);
        }

        /*
         * Protected.
         */

        protected abstract void OnMapReturn(T map);
        protected abstract T    CreateMap();
    }
}