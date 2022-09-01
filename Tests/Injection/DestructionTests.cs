using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
{
    public sealed class DestructionTests
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
        public void Destruction()
        {
            InjectionBinder.Destroy(InjectionBinder.Construct<Controller>(true), true);
            Assert.AreEqual(1, Controller.destroyedTimes);
        }

        [Test]
        public void NoPreDestroy()
        {
            InjectionBinder.Destroy(InjectionBinder.Construct<Controller>(true), false);
            Assert.AreEqual(0, Controller.destroyedTimes);
        }

        [Test]
        public void FalseDestruction()
        {
            InjectionBinder.Destroy(new Controller(), true);
            Assert.AreEqual(0, Controller.destroyedTimes);
        }
    }
}