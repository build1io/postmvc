using Build1.PostMVC.Core.MVCS.Injection;
using Build1.PostMVC.Core.MVCS.Injection.Impl;
using Build1.PostMVC.Core.Tests.Injection.Parts;
using NUnit.Framework;

namespace Build1.PostMVC.Core.Tests.Injection
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
            Assert.True(InjectionBinder.Get<Controller>() == InjectionBinder.Get<Controller>());
            
            InjectionBinder.Rebind<IController>().To<Controller>();
            Assert.True(InjectionBinder.Get<IController>() == InjectionBinder.Get<IController>());
        }
        
        /*
         * Factory Mode.
         */
        
        [Test]
        public void FactoryInjection()
        {
            InjectionBinder.Bind<Controller>().AsFactory();
            Assert.True(InjectionBinder.Get<Controller>() != InjectionBinder.Get<Controller>());
            
            InjectionBinder.Rebind<IController, Controller>().AsFactory();
            Assert.True(InjectionBinder.Get<IController>() != InjectionBinder.Get<IController>());
        }
        
        /*
         * Singleton Mode.
         */
        
        [Test]
        public void SingletonInjection()
        {
            InjectionBinder.Bind<Controller>().AsSingleton();
            Assert.True(InjectionBinder.Get<Controller>() == InjectionBinder.Get<Controller>());
            
            InjectionBinder.Rebind<IController, Controller>().AsSingleton();
            Assert.True(InjectionBinder.Get<IController>() == InjectionBinder.Get<IController>());
        }
        
        /*
         * Interface Construction.
         */

        [Test]
        public void InterfaceConstruction()
        {
            InjectionBinder.Bind<IController>().AsFactory();
            Assert.That(() => InjectionBinder.Get<IController>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<IController>().AsSingleton();
            Assert.That(() => InjectionBinder.Get<IController>(), Throws.TypeOf<InjectionException>());
        }
        
        /*
         * Struct Injection.
         */

        [Test]
        public void PrimitiveInjection()
        {
            InjectionBinder.Bind<int>();
            Assert.AreEqual(InjectionBinder.Get<int>(), InjectionBinder.Get<int>());
            
            InjectionBinder.Rebind<int>().AsFactory();
            Assert.AreEqual(InjectionBinder.Get<int>(), InjectionBinder.Get<int>());
            
            InjectionBinder.Rebind<int>().AsSingleton();
            Assert.AreEqual(InjectionBinder.Get<int>(), InjectionBinder.Get<int>());
        }
        
        [Test]
        public void StringInjection()
        {
            InjectionBinder.Bind<string>();
            Assert.That(() => InjectionBinder.Get<string>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<string>().AsFactory();
            Assert.That(() => InjectionBinder.Get<string>(), Throws.TypeOf<InjectionException>());
            
            InjectionBinder.Rebind<string>().AsSingleton();
            Assert.That(() => InjectionBinder.Get<string>(), Throws.TypeOf<InjectionException>());
        }
        
        [Test]
        public void EnumInjection()
        {
            InjectionBinder.Bind<InjectionEnum>();
            Assert.AreEqual(InjectionBinder.Get<InjectionEnum>(), InjectionEnum.Value01);
            
            InjectionBinder.Rebind<InjectionEnum>().AsFactory();
            Assert.AreEqual(InjectionBinder.Get<InjectionEnum>(), InjectionEnum.Value01);
            
            InjectionBinder.Rebind<InjectionEnum>().AsSingleton();
            Assert.AreEqual(InjectionBinder.Get<InjectionEnum>(), InjectionEnum.Value01);
        }
        
        [Test]
        public void StructInjection()
        {
            InjectionBinder.Bind<Struct>();
            Assert.AreEqual(InjectionBinder.Get<Struct>(), InjectionBinder.Get<Struct>());
            
            InjectionBinder.Rebind<Struct>().AsFactory();
            Assert.AreEqual(InjectionBinder.Get<Struct>(), InjectionBinder.Get<Struct>());
            
            InjectionBinder.Rebind<Struct>().AsSingleton();
            Assert.AreEqual(InjectionBinder.Get<Struct>(), InjectionBinder.Get<Struct>());
        }
        
        [Test]
        public void StructConstruction()
        {
            InjectionBinder.Bind<Struct>();
            InjectionBinder.Get<Struct>();
            Assert.AreEqual(Struct.constructedTimes, 1);
        }
    }
}