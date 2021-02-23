using System.Collections;
using System.Collections.Generic;

namespace Build1.PostMVC.Extensions.Unity.Modules.UI
{
    public abstract class UIControl<T> : IEnumerable<T> where T : UIControlConfiguration
    {
        public readonly string            name;
        public readonly UIControlBehavior behavior;

        public bool ToPreInstantiate        => (behavior & UIControlBehavior.PreInstantiate) == UIControlBehavior.PreInstantiate;
        public bool ToDestroyOnDeactivation => (behavior & UIControlBehavior.DestroyOnDeactivation) == UIControlBehavior.DestroyOnDeactivation;

        private readonly IList<T> _configurations;

        protected UIControl(string name, UIControlBehavior behavior)
        {
            this.name = name;
            this.behavior = behavior;

            _configurations = new List<T>();
        }

        public void Add(T configuration)
        {
            _configurations.Add(configuration);
        }

        public T First()
        {
            return _configurations[0];
        }

        public IEnumerator<T> GetEnumerator()
        {
            return _configurations.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public override string ToString()
        {
            return name;
        }
    }
}