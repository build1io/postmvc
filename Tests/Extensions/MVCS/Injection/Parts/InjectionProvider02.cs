using System.Collections.Generic;
using Build1.PostMVC.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts
{
    public sealed class InjectionProvider02 : InjectionProvider<IInjectionProviderItem>
    {
        public int CounterCreatedInstances { get; private set; }
        public int CounterTaken            { get; private set; }
        public int CounterReturned         { get; private set; }

        private readonly Stack<IInjectionProviderItem> _availableInstances = new();
        private readonly List<IInjectionProviderItem>  _usedInstances = new();
        
        public override IInjectionProviderItem TakeInstance(object parent, Inject attribute)
        {
            IInjectionProviderItem item;

            if (_availableInstances.Count > 0)
            {
                item = _availableInstances.Pop();
                _usedInstances.Add(item);
            }
            else
            {
                item = new InjectionProviderItem02();
                _usedInstances.Add(item);

                CounterCreatedInstances++;
            }
            
            CounterTaken++;
            return item;
        }

        public override void ReturnInstance(IInjectionProviderItem instance)
        {
            if (!_usedInstances.Remove(instance))
                return;
                
            _availableInstances.Push(instance);
            CounterReturned++;
        }
    }
}