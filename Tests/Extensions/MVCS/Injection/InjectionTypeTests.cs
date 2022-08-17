using Build1.PostMVC.Extensions.MVCS.Injection;
using Build1.PostMVC.Extensions.MVCS.Injection.Impl;
using Build1.PostMVC.Tests.Extensions.MVCS.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Tests.Extensions.MVCS.Injection
{
    public sealed class InjectionTypeTests
    {
        private IInjectionBinder InjectionBinder;
        
        [SetUp]
        public void SetUp()
        {
            InjectionBinder = new InjectionBinder();
            
            Struct.constructedTimes = 0;
            Controller.constructedTimes = 0;
            Controller.destroyedTimes = 0;
        }
        
        /*
         * Default Behavior.
         */
        
        [Test]
        public void DefaultInjectionBehavior()
        {
            InjectionBinder.Bind<Controller>();
            Assert.True(InjectionBinder.GetInstance<Controller>() == InjectionBinder.GetInstance<Controller>());
            
            InjectionBinder.Rebind<IController>().To<Controller>();
            Assert.True(InjectionBinder.GetInstance<IController>() == InjectionBinder.GetInstance<IController>());
        }
        
        /*
         * Factory Mode.
         */
        
        [Test]
        public void FactoryInjection()
        {
            InjectionBinder.Bind<Controller>().AsFactory();
            Assert.True(InjectionBinder.GetInstance<Controller>() != InjectionBinder.GetInstance<Controller>());
            
            InjectionBinder.Rebind<IController, Controller>().AsFactory();
            Assert.True(InjectionBinder.GetInstance<IController>() != InjectionBinder.GetInstance<IController>());
        }
        
        /*
         * Singleton Mode.
         */
        
        [Test]
        public void SingletonInjection()
        {
            InjectionBinder.Bind<Controller>().AsSingleton();
            Assert.True(InjectionBinder.GetInstance<Controller>() == InjectionBinder.GetInstance<Controller>());
            
            InjectionBinder.Rebind<IController, Controller>().AsSingleton();
            Assert.True(InjectionBinder.GetInstance<IController>() == InjectionBinder.GetInstance<IController>());
        }
        
        /*
         * Interface Construction.
         */

        [Test]
        public void InterfaceConstruction()
        {
            InjectionBinder.Bind<IController>().AsFactory();
            Assert.That(() => InjectionBinder.GetInstance<IController>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<IController>().AsSingleton();
            Assert.That(() => InjectionBinder.GetInstance<IController>(), Throws.TypeOf<InjectionException>());
        }
        
        /*
         * Struct Injection.
         */

        [Test]
        public void PrimitiveInjection()
        {
            InjectionBinder.Bind<int>();
            Assert.AreEqual(InjectionBinder.GetInstance<int>(), InjectionBinder.GetInstance<int>());
            
            InjectionBinder.Rebind<int>().AsFactory();
            Assert.AreEqual(InjectionBinder.GetInstance<int>(), InjectionBinder.GetInstance<int>());
            
            InjectionBinder.Rebind<int>().AsSingleton();
            Assert.AreEqual(InjectionBinder.GetInstance<int>(), InjectionBinder.GetInstance<int>());
        }
        
        [Test]
        public void StringInjection()
        {
            InjectionBinder.Bind<string>();
            Assert.That(() => InjectionBinder.GetInstance<string>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<string>().AsFactory();
            Assert.That(() => InjectionBinder.GetInstance<string>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<string>().AsSingleton();
            Assert.That(() => InjectionBinder.GetInstance<string>(), Throws.TypeOf<InjectionException>());
        }
        
        [Test]
        public void EnumInjection()
        {
            InjectionBinder.Bind<InjectionEnum>();
            Assert.AreEqual(InjectionBinder.GetInstance<InjectionEnum>(), InjectionEnum.Value01);
            
            InjectionBinder.Rebind<InjectionEnum>().AsFactory();
            Assert.AreEqual(InjectionBinder.GetInstance<InjectionEnum>(), InjectionEnum.Value01);
            
            InjectionBinder.Rebind<InjectionEnum>().AsSingleton();
            Assert.AreEqual(InjectionBinder.GetInstance<InjectionEnum>(), InjectionEnum.Value01);
        }
        
        [Test]
        public void StructInjection()
        {
            InjectionBinder.Bind<Struct>();
            Assert.AreEqual(InjectionBinder.GetInstance<Struct>(), InjectionBinder.GetInstance<Struct>());
            
            InjectionBinder.Rebind<Struct>().AsFactory();
            Assert.AreEqual(InjectionBinder.GetInstance<Struct>(), InjectionBinder.GetInstance<Struct>());
            
            InjectionBinder.Rebind<Struct>().AsSingleton();
            Assert.AreEqual(InjectionBinder.GetInstance<Struct>(), InjectionBinder.GetInstance<Struct>());
        }
        
        [Test]
        public void StructConstruction()
        {
            InjectionBinder.Bind<Struct>();
            InjectionBinder.GetInstance<Struct>();
            Assert.AreEqual(Struct.constructedTimes, 1);
        }
    }
}