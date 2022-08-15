using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection
{
    public sealed class LifeCycleTests
    {
        private IInjectionBinder InjectionBinder;
        
        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();
            
            Controller.constructedTimes = 0;
            Controller.destroyedTimes = 0;
        }
        
        /*
         * Default Behavior.
         */
        
        [Test]
        public void PostConstruct()
        {
            InjectionBinder.Construct<Controller>(false);
            Assert.AreEqual(Controller.constructedTimes, 0);
            
            InjectionBinder.Construct<Controller>(true);
            Assert.AreEqual(Controller.constructedTimes, 1);
        }

        [Test]
        public void PreDestroy()
        {
            var instance = InjectionBinder.Construct<Controller>(false);
            InjectionBinder.Destroy(instance, false);
            Assert.AreEqual(Controller.destroyedTimes, 0);
            
            instance = InjectionBinder.Construct<Controller>(false);
            InjectionBinder.Destroy(instance, true);
            Assert.AreEqual(Controller.destroyedTimes, 1);
        }
    }
}