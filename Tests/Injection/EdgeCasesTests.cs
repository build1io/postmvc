using Build1.PostMVC.Core.Extensions.MVCS.Injection;
using Build1.PostMVC.Core.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
{
    public sealed class EdgeCasesTests
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
        public void DoubleConstruction1()
        {
            var controller = new Controller();

            InjectionBinder.Bind<IController>().ToValue(controller);
            Assert.AreEqual(0, Controller.constructedTimes);

            InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(0, Controller.constructedTimes);

            InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(0, Controller.constructedTimes);
        }

        [Test]
        public void DoubleConstruction2()
        {
            var instance = InjectionBinder.Construct<Controller>(true);
            Assert.AreEqual(1, Controller.constructedTimes);

            InjectionBinder.Bind<IController>().ToValue(instance);
            Assert.AreEqual(1, Controller.constructedTimes);

            InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(1, Controller.constructedTimes);
        }

        [Test]
        public void DoubleConstruction3()
        {
            InjectionBinder.Bind<IController, Controller>();
            Assert.AreEqual(Controller.constructedTimes, 0);

            var instance = InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(1, Controller.constructedTimes);

            instance = InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(1, Controller.constructedTimes);

            instance = InjectionBinder.Construct(instance, true);
            Assert.AreEqual(1, Controller.constructedTimes);
        }

        [Test]
        public void DoubleConstruction4()
        {
            InjectionBinder.Bind<IController, Controller>();
            Assert.AreEqual(Controller.constructedTimes, 0);

            var instance = InjectionBinder.GetInstance<IController>();
            Assert.AreEqual(1, Controller.constructedTimes);

            InjectionBinder.Bind<Controller>().ToValue(instance);
            InjectionBinder.GetInstance<Controller>();
            Assert.AreEqual(1, Controller.constructedTimes);
        }
    }
}