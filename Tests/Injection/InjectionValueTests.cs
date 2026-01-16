using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
{
    public sealed class InjectionValueTests
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
         * Default Injection.
         */
        
        [Test]
        public void DefaultInjectionBehavior()
        {
            var controller = new Controller();
            
            InjectionBinder.Bind(controller);
            Assert.AreEqual(controller, InjectionBinder.Get<Controller>());
            
            InjectionBinder.Rebind<Controller>().ToValue(controller);
            Assert.AreEqual(controller, InjectionBinder.Get<Controller>());
        }
        
        [Test]
        public void DefaultInjectionBehaviorWithInterface()
        {
            var controller = new Controller();
            
            InjectionBinder.Bind<IController>(controller);
            Assert.AreEqual(controller, InjectionBinder.Get<IController>());
            
            InjectionBinder.Rebind<IController>().ToValue(controller);
            Assert.AreEqual(controller, InjectionBinder.Get<IController>());
        }

        /*
         * Value Construction.
         */
        
        [Test]
        public void ValueConstruction()
        {
            var controller = new Controller();
            
            InjectionBinder.Bind(controller);
            InjectionBinder.Get<Controller>();
            Assert.AreEqual(Controller.constructedTimes, 0);
            
            InjectionBinder.Rebind(controller).ConstructValue();
            InjectionBinder.Get<Controller>();
            Assert.AreEqual(Controller.constructedTimes, 1);
        }

        [Test]
        public void ValueConstructionViaInterface()
        {
            var controller = new Controller();
            
            InjectionBinder.Bind<IController>(controller);
            InjectionBinder.Get<IController>();
            Assert.AreEqual(Controller.constructedTimes, 0);
            
            InjectionBinder.Rebind<IController>(controller).ConstructValue();
            InjectionBinder.Get<IController>();
            Assert.AreEqual(Controller.constructedTimes, 1);
        }
        
        /*
         * Simple Types.
         */
        
        [Test]
        public void PrimitiveInjection()
        {
            InjectionBinder.Bind(10);
            Assert.AreEqual(InjectionBinder.Get<int>(), 10);
        }
        
        [Test]
        public void StringInjection()
        {
            InjectionBinder.Bind("12345");
            Assert.AreEqual(InjectionBinder.Get<string>(), "12345");
        }
        
        [Test]
        public void EnumInjection()
        {
            InjectionBinder.Bind(InjectionEnum.Value02);
            Assert.AreEqual(InjectionBinder.Get<InjectionEnum>(), InjectionEnum.Value02);
        }
        
        [Test]
        public void StructInjection()
        {
            var str = new Struct(); 
            InjectionBinder.Bind(str);
            Assert.AreEqual(InjectionBinder.Get<Struct>(), str);
        }
        
        /*
         * Other.
         */

        [Test]
        public void FalseConstruction01()
        {
            InjectionBinder.Bind<IController>().To(new Controller());
            InjectionBinder.Get<IController>();
            Assert.AreEqual(0, Controller.constructedTimes);
        }
        
        [Test]
        public void FalseConstruction02()
        {
            InjectionBinder.Bind<IController>().ToValue(new Controller());
            InjectionBinder.Get<IController>();
            Assert.AreEqual(0, Controller.constructedTimes);
        }
        
        [Test]
        public void FalseConstruction03()
        {
            InjectionBinder.Bind<IController>(new Controller());
            InjectionBinder.Get<IController>();
            Assert.AreEqual(0, Controller.constructedTimes);
        }
    }
}