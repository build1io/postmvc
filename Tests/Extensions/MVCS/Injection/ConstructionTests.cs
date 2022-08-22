using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection
{
    public sealed class ConstructionTests
    {
        private IInjectionBinder InjectionBinder;

        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();

            Controller.constructedTimes = 0;
            Controller.destroyedTimes = 0;
        }

        [Test]
        public void ConstructionByType()
        {
            Assert.NotNull(InjectionBinder.Construct<Controller>(true));
            Assert.AreEqual(1, Controller.constructedTimes);
        }

        [Test]
        public void ConstructionByValue()
        {
            InjectionBinder.Construct(new Controller(), true);
            Assert.AreEqual(1, Controller.constructedTimes);
        }

        [Test]
        public void ConstructionOfAlreadyConstructed()
        {
            var instance = new Controller();
            
            InjectionBinder.Construct(instance, true);
            Assert.AreEqual(1, Controller.constructedTimes);
            
            InjectionBinder.Construct(instance, true);
            Assert.AreEqual(1, Controller.constructedTimes);
        }

        [Test]
        public void NoPostConstructByType()
        {
            Assert.NotNull(InjectionBinder.Construct<Controller>(false));
            Assert.AreEqual(0, Controller.constructedTimes);
        }
        
        [Test]
        public void NoPostConstructByValue()
        {
            InjectionBinder.Construct(new Controller(), false);
            Assert.AreEqual(0, Controller.constructedTimes);
        }
    }
}