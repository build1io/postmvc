using System.Collections.Generic;
using Build1.PostMVC.Core.Extensions.MVCS.Injection;

namespace Build1.PostMVC.Core.Tests.Injection.Parts
{
    public sealed class InjectionProvider01 : InjectionProvider<IInjectionProviderItem>
    {
        public static int Constructed   { get; set; }
        public static int Destroyed     { get; set; }
        public static int ItemsCreated  { get; set; }
        public static int ItemsTaken    { get; set; }
        public static int ItemsReturned { get; set; }

        private Stack<IInjectionProviderItem> _availableInstances;
        private List<IInjectionProviderItem>  _usedInstances;

        [PostConstruct]
        public void PostConstruct()
        {
            _availableInstances = new Stack<IInjectionProviderItem>();
            _usedInstances = new List<IInjectionProviderItem>();

            Constructed++;
        }

        [PreDestroy]
        public void PreDestroy()
        {
            _availableInstances = null;
            _usedInstances = null;

            Destroyed++;
        }

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
                item = new InjectionProviderItem01();
                _usedInstances.Add(item);

                ItemsCreated++;
            }

            ItemsTaken++;
            return item;
        }

        public override void ReturnInstance(IInjectionProviderItem instance)
        {
            if (!_usedInstances.Remove(instance))
                return;

            _availableInstances.Push(instance);
            ItemsReturned++;
        }
    }
}